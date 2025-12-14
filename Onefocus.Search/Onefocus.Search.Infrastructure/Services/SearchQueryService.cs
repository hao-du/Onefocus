using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using OpenSearch.Net;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services;

public class SearchQueryService(IOpenSearchClient client, ILogger<SearchIndexService> logger)
{
    public async Task<Results.Result<string>> SearchAsync(SearchQueryDto searchQueryDto, IEmbeddingService embeddingService, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(searchQueryDto.IndexName)) return Results.Result.Failure<string>(Errors.IndexIsRequired);
        if (searchQueryDto.Query == null) return Results.Result.Failure<string>(Errors.QueryIsRequired);

        string searchJson = JsonHelper.SerializeJson(searchQueryDto.Query);

        if (searchQueryDto.VectorSearchTerms.Count > 0)
        {
            var vectorSearchTerms = searchQueryDto.VectorSearchTerms.Select(t => t.Value).ToList();
            var getEmbeddingResult = await embeddingService.GetEmbeddings(new(vectorSearchTerms), cancellationToken);
            if (getEmbeddingResult.IsFailure) return getEmbeddingResult.Failure<string>();

            var embeddings = getEmbeddingResult.Value.Results.ToDictionary(r => r.Text, r => r.Embeddings);
            searchJson = JsonHelper.AppendEmbeddings(searchJson, searchQueryDto.VectorSearchTerms, embeddings);
        }

        var response = await client.LowLevel.SearchAsync<StringResponse>(searchQueryDto.IndexName, PostData.String(searchJson), ctx: cancellationToken);
        if (!response.Success)
        {
            logger.LogError("Perform search failed: {Status} {Response}", response.HttpStatusCode, response.Body);
            return Results.Result.Failure<string>(Errors.PerformSearchError);
        }

        return response.Body;
    }
}