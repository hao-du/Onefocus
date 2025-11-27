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
    public class SearchIndexService(IOpenSearchClient client, ISearchSettings searchSettings, ILogger<SearchIndexService> logger) : ISearchIndexService
    {
        public async Task<Results.Result> IndexEntities(IReadOnlyList<SearchIndexDocumentDto> documentDtos, CancellationToken cancellationToken = default)
        {
            try
            {
                var bulkDescriptor = new BulkDescriptor();

                var sb = new StringBuilder();
                foreach (var doc in documentDtos)
                {
                    var rawJson = ((JsonElement)doc.Payload).GetRawText();
                    var jsonDoc = JsonDocument.Parse(rawJson);
                    string minifiedJson = JsonSerializer.Serialize(jsonDoc.RootElement);

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
}
