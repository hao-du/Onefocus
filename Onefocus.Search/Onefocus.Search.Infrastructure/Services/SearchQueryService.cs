using Microsoft.Extensions.Logging;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using OpenSearch.Net;
using System.Text.Json;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services;

public class SearchQueryService(IOpenSearchClient client, ILogger<SearchIndexService> logger, IEmbeddingService embeddingService) : ISearchQueryService
{
    public async Task<Results.Result<string>> SearchAsync(Application.Contracts.GraphQL.SearchRequest searchQueryDto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(searchQueryDto.IndexName)) return Results.Result.Failure<string>(Errors.IndexIsRequired);
        if (searchQueryDto.Filter == null) return Results.Result.Failure<string>(Errors.QueryIsRequired);

        var searchJsonResult = await JsonQueryBuilderAsync(searchQueryDto, cancellationToken);
        if (searchJsonResult.IsFailure) return searchJsonResult.Failure<string>();

        var response = await client.LowLevel.SearchAsync<StringResponse>(searchQueryDto.IndexName, PostData.String(searchJsonResult.Value), ctx: cancellationToken);
        if (!response.Success)
        {
            logger.LogError("Perform search failed: {Status} {Response}", response.HttpStatusCode, response.Body);
            return Results.Result.Failure<string>(Errors.PerformSearchError);
        }

        return TransformResult(response.Body);
    }

    private async Task<Result<string>> JsonQueryBuilderAsync(Application.Contracts.GraphQL.SearchRequest searchQueryDto, CancellationToken cancellationToken = default)
    {
        var filters = new List<object>();
        var matches = new List<object>();

        if (searchQueryDto.Filter.RangeFilters != null && searchQueryDto.Filter.RangeFilters.Count > 0)
        {
            foreach (var range in searchQueryDto.Filter.RangeFilters)
            {
                var rangeBody = new Dictionary<string, object?>();

                if (range.Gte != null) rangeBody["gte"] = range.Gte;
                if (range.Lte != null) rangeBody["lte"] = range.Lte;

                filters.Add(new
                {
                    range = new Dictionary<string, object?>
                    {
                        [range.Field] = rangeBody
                    }
                });
            }
        }

        if (searchQueryDto.Filter.TermFilters != null && searchQueryDto.Filter.TermFilters.Count > 0)
        {
            foreach (var term in searchQueryDto.Filter.TermFilters)
            {
                filters.Add(new
                {
                    term = new Dictionary<string, object?>
                    {
                        [term.Field] = term.Value
                    }
                });
            }
        }

        if (searchQueryDto.Filter.MatchFilters != null && searchQueryDto.Filter.MatchFilters.Count > 0)
        {
            foreach (var term in searchQueryDto.Filter.MatchFilters)
            {
                matches.Add(new
                {
                    match = new Dictionary<string, string>
                    {
                        [term.Field] = term.Value
                    }
                });
            }
        }

        if (searchQueryDto.Filter.SemanticFilters != null && searchQueryDto.Filter.SemanticFilters.Count > 0)
        {
            var vectorSearchTerms = searchQueryDto.Filter.SemanticFilters.Select(t => t.Value).ToList();
            var getEmbeddingResult = await embeddingService.GetEmbeddings(new(vectorSearchTerms), cancellationToken);
            if (getEmbeddingResult.IsFailure) return getEmbeddingResult.Failure<string>();

            var embeddings = getEmbeddingResult.Value.Results.ToDictionary(r => r.Text, r => r.Embeddings);

            foreach (var term in searchQueryDto.Filter.SemanticFilters)
            {
                matches.Add(new
                {
                    knn = new Dictionary<string, object>
                    {
                        [term.Field] = new
                        {
                            vertor = embeddings[term.Value],
                            k = 10
                        }
                    }

                });
            }
        }

        var openSearchQuery = new
        {
            size = searchQueryDto.Paging == null ? 20 : searchQueryDto.Paging.Take,
            query = new
            {
                @bool = new
                {
                    minimum_should_match = matches.Count > 1 ? 1 : 0,
                    should = matches,
                    filter = filters
                }
            }
        };

        return JsonHelper.SerializeJson(openSearchQuery);
    }

    public string TransformResult(string searchResultBody)
    {
        using JsonDocument doc = JsonDocument.Parse(searchResultBody);
        var root = doc.RootElement;

        var hits = root.GetProperty("hits");
        var total = hits.GetProperty("total").GetProperty("value").GetInt32();
        var maxScore = hits.GetProperty("max_score").GetDouble();

        var data = new List<Dictionary<string, object>>();

        foreach (var hit in hits.GetProperty("hits").EnumerateArray())
        {
            var source = hit.GetProperty("_source");
            var score = hit.GetProperty("_score").GetDouble();

            var item = new Dictionary<string, object>();
            foreach (var property in source.EnumerateObject())
            {
                if (property.Name == "embedding")
                    continue;

                item[property.Name] = GetJsonValue(property.Value);
            }
            item["score"] = score;

            data.Add(item);
        }

        var result = new
        {
            total,
            max_score = maxScore,
            data
        };

        return JsonHelper.SerializeJson(result);
    }

    private object GetJsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.TryGetInt32(out var i) ? (object)i : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => string.Empty,
            JsonValueKind.Array => element.EnumerateArray().Select(GetJsonValue).ToList(),
            JsonValueKind.Object => element.EnumerateObject().ToDictionary(p => p.Name, p => GetJsonValue(p.Value)),
            _ => element.ToString()
        };
    }
}