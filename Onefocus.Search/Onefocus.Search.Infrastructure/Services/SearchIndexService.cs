using Microsoft.Extensions.Logging;
using Onefocus.Common.Utilities;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using OpenSearch.Client;
using OpenSearch.Net;
using System.Text;
using Results = Onefocus.Common.Results;

namespace Onefocus.Search.Infrastructure.Services;

public class SearchIndexService(IOpenSearchClient client, IEmbeddingService embeddingService, ILogger<SearchIndexService> logger) : ISearchIndexService
{
    public async Task<Results.Result> IndexEntities(IReadOnlyList<SearchIndexDocumentDto> documentDtos, CancellationToken cancellationToken = default)
    {
        try
        {
            var vectorSearchTerms = documentDtos.SelectMany(d => d.VectorSearchTerms.Select(t => t.Value)).Distinct().ToList();
            var embeddings = new Dictionary<string, List<float>>();
            if (vectorSearchTerms.Count > 0)
            {
                var getEmbeddingResult = await embeddingService.GetEmbeddings(new(vectorSearchTerms), cancellationToken);
                if (getEmbeddingResult.IsFailure) return getEmbeddingResult;

                embeddings = getEmbeddingResult.Value.Results.ToDictionary(r => r.Text, r => r.Embeddings);
            }

            var bulkDescriptor = new BulkDescriptor();
            var sb = new StringBuilder();
            foreach (var doc in documentDtos)
            {
                string minifiedJson = JsonHelper.SerializeJson(doc.Payload);
                minifiedJson = JsonHelper.AppendEmbeddings(minifiedJson, doc.VectorSearchTerms, embeddings);

                // action/metadata line
                sb.AppendLine($@"{{ ""index"": {{ ""_index"": ""{doc.IndexName}"", ""_id"": ""{doc.EntityId}"" }} }}");
                // source line (raw JSON)
                sb.AppendLine(minifiedJson);
            }

            var bulkString = sb.ToString();
            var response = await client.LowLevel.BulkAsync<StringResponse>(PostData.String(bulkString), ctx: cancellationToken);


            if (!response.Success)
            {
                logger.LogError("Low-level bulk failed: {Status} {Response}", response.HttpStatusCode, response.Body);
                return Results.Result.Failure(Errors.IndexError);
            }

            logger.LogInformation("Successfully bulk indexed documents.");
            return Results.Result.Success();
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Exception bulk indexing documents.");
            return Results.Result.Failure("IndexException", ex.Message);
        }
    }
}
