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

                foreach (var document in documentDtos)
                {
                    bulkDescriptor.Index<object>(i => i
                        .Index(document.IndexName)
                        .Id(document.EntityId)
                        .Document(document.Payload)
                    );
                }

                var response = await client.BulkAsync(bulkDescriptor, ct: cancellationToken);

                if (!response.IsValid || response.Errors)
                {
                    foreach (var item in response.ItemsWithErrors)
                    {
                        logger.LogError("Error bulk indexing document {DocumentId}: {Error}", item.Id, item.Error?.Reason);
                    }
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
