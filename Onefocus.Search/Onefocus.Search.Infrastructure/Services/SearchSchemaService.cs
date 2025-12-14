using Microsoft.Extensions.Logging;
using Onefocus.Common.Utilities;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using OpenSearch.Net;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services;

public class SearchSchemaService(IOpenSearchClient client, ILogger<SearchIndexService> logger) : ISearchSchemaService
{
    public async Task<Results.Result> UpsertIndexMappings(SearchSchemaDto searchSchemaDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(searchSchemaDto.IndexName))
            return Results.Result.Failure(Errors.IndexIsRequired);

        var existsResponse = await client.Indices.ExistsAsync(searchSchemaDto.IndexName, ct: cancellationToken);
        if (existsResponse.Exists)
            return await UpdateIndexMappings(searchSchemaDto, cancellationToken);

        return await AddIndexMappings(searchSchemaDto, cancellationToken);
    }

    private async Task<Results.Result> AddIndexMappings(SearchSchemaDto searchSchemaDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating index: {IndexName}", searchSchemaDto.IndexName);

        try
        {
            var schema = JsonHelper.SerializeJson(searchSchemaDto.Mappings);

            var response = await client.LowLevel.Indices.CreateAsync<StringResponse>(
                index: searchSchemaDto.IndexName,
                body: PostData.String(schema),
                ctx: cancellationToken
            );

            if (!response.Success)
            {
                Log(searchSchemaDto.IndexName, response);
                return Results.Result.Failure(Errors.InvalidIndexCreation);
            }

            return Results.Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception creating index {IndexName}", searchSchemaDto.IndexName);
            return Results.Result.Failure("IndexException", ex.Message);
        }
    }

    private async Task<Results.Result> UpdateIndexMappings(SearchSchemaDto searchSchemaDto, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating mappings for index: {IndexName}", searchSchemaDto.IndexName);

        try
        {
            var schemaSectionDictionary = JsonHelper.GetSections(searchSchemaDto.Mappings, ["mappings"]);

            var mappingsResponse = await client.LowLevel.Indices.PutMappingAsync<StringResponse>(
                index: searchSchemaDto.IndexName,
                body: schemaSectionDictionary["mappings"],
                ctx: cancellationToken
            );
            if (!mappingsResponse.Success)
            {
                Log(searchSchemaDto.IndexName, mappingsResponse);
                return Results.Result.Failure(Errors.InvalidMappingsUpdate);
            }

            return Results.Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception creating index {IndexName}", searchSchemaDto.IndexName);
            return Results.Result.Failure("IndexException", ex.Message);
        }
    }

    private void Log(string indexName, StringResponse response)
    {
        if (response.TryGetServerError(out var serverError))
        {
            logger.LogError("Schema {indexName} failed with statusCode: {status}, reason: {reason}",
                indexName,
                serverError.Status,
                serverError.Error?.Reason
            );
            if (serverError.Error?.RootCause != null)
            {
                foreach (var rootCause in serverError.Error.RootCause)
                {
                    logger.LogError("Schema {indexName} failed with root reason: {reason}",
                        indexName,
                        rootCause.Reason
                    );
                }
            }
        }
        logger.LogError("Schema {indexName} failed with statusCode: {status}, response: {response}",
            indexName,
            response.HttpStatusCode,
            response.Body
        );
    }
}
