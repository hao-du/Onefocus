using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Configurations;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using OpenSearch.Net;
using System.Text;
using System.Text.Json;
using Error = Onefocus.Common.Results.Error;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services
{
    public class IndexingService(IOpenSearchClient client, ISearchSettings searchSettings, ILogger<IndexingService> logger) : IIndexingService
    {
        public async Task<Results.Result> AddIndex(IReadOnlyList<SearchIndexDto> envelopeDtos, CancellationToken cancellationToken = default)
        {
            var envelopGroups = envelopeDtos
                .Where(e => e.EntityId != null && e.Payload.ValueKind != JsonValueKind.Undefined)
                .GroupBy(e => GetIndexName(e));

            foreach (var envelopGroup in envelopGroups)
            {
                var index = envelopGroup.Key;
                var ensureResult = await EnsureIndex(index);
                if (ensureResult.IsFailure) return ensureResult;

                var sb = new StringBuilder();
                foreach (var envelop in envelopGroup)
                {
                    sb.AppendLine(JsonSerializer.Serialize(new { index = new { _index = index, _id = envelop.EntityId } }));
                    sb.AppendLine(envelop.Payload.GetRawText());
                }

                var bulkBody = sb.ToString();
                var bulkResponse = await client.LowLevel.BulkAsync<StringResponse>(PostData.String(bulkBody), ctx: cancellationToken);
                if (!bulkResponse.Success)
                    return Results.Result.Failure("IndexError", GetError(bulkResponse));
            }

            return Results.Result.Success();
        }

        private async Task<Results.Result> EnsureIndex(string index)
        {
            if (string.IsNullOrWhiteSpace(index))
                return Results.Result.Failure(Errors.IndexIsRequired);

            var existsResponse = await client.Indices.ExistsAsync(index);
            if (!existsResponse.IsValid)
                return Results.Result.Failure("EnsureIndexExists", GetError(existsResponse));
            if (existsResponse.Exists)
                return Results.Result.Success();

            var createResponse = await client.Indices.CreateAsync(index, c => c
                .Settings(s => s.NumberOfShards(1).NumberOfReplicas(0))
                .Map(m => m.Dynamic(true))
            );

            if (!createResponse.IsValid)
                return Results.Result.Failure("EnsureIndexExists", GetError(createResponse));

            return Results.Result.Success();
        }

        private string GetIndexName(SearchIndexDto env)
        {
            if (!string.IsNullOrWhiteSpace(env.EntityType))
                return $"index_{env.EntityType!.Trim().ToLowerInvariant()}";

            if (!string.IsNullOrWhiteSpace(searchSettings.DefaultIndexName))
                return searchSettings.DefaultIndexName;

            return "index_default";
        }

        private string GetError(ResponseBase responseBase)
        {
            if (responseBase.ServerError.Error.CausedBy != null)
            {
                logger.LogError("Index failed with CausedBy error: {Reason}.", responseBase.ServerError.Error.CausedBy.Reason);
                return responseBase.ServerError.Error.CausedBy.Reason;
            }

            if (responseBase.ServerError.Error.Reason != null)
            {
                logger.LogError("Index failed with error: {Reason}.", responseBase.ServerError.Error.Reason);
                return responseBase.ServerError.Error.Reason;
            }

            if (responseBase.OriginalException != null)
            {
                logger.LogError("Index failed with original exception: {Message}.", responseBase.OriginalException.Message);
                return responseBase.OriginalException.Message;
            }

            logger.LogError("Index failed with unknown error.");
            return "Unknown error";
        }

        private string GetError(StringResponse response)
        {
            if (response.TryGetServerError(out var serverError))
            {
                if (serverError.Error.CausedBy != null)
                {
                    logger.LogError("Index failed with CausedBy error: {Reason}.", serverError.Error.CausedBy.Reason);
                    return serverError.Error.CausedBy.Reason;
                }
                if (serverError.Error.Reason != null)
                {
                    logger.LogError("Index failed with error: {Reason}.", serverError.Error.Reason);
                    return serverError.Error.Reason;
                }
            }

            if (response.OriginalException != null)
            {
                logger.LogError("Index failed with original exception: {Message}.", response.OriginalException.Message);
                return response.OriginalException.Message;
            }

            logger.LogError("Index failed with unknown error.");
            return "Unknown error";
        }
    }
}
