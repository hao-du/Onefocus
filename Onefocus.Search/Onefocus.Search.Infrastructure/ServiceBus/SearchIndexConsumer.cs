using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Services;
using Onefocus.Search.Infrastructure.Services;
using System.Text.Json;
using System.Threading;

namespace Onefocus.Search.Infrastructure.ServiceBus;

internal class SearchIndexConsumer(
    ISearchIndexService indexingService,
    ILogger<SearchIndexConsumer> logger
    ) : IConsumer<ISearchIndexMessage>
{
    public async Task Consume(ConsumeContext<ISearchIndexMessage> context)
    {
        var searchIndexDtos = context.Message.Documents.Select(entity => new SearchIndexDocumentDto(
            EntityId: entity.DocumentId,
            IndexName: entity.IndexName,
            Payload: entity.Payload
        )).ToList();

        var indexResult = await indexingService.IndexEntities(searchIndexDtos, context.CancellationToken);

        if (indexResult.IsFailure)
            LogError(indexResult);
    }

    private void LogError(Result result)
    {
        foreach (var error in result.Errors)
        {
            logger.LogError("Error when bulk-indexing with Code: {Code}, Description: {Description}", error.Code, error.Description);
        }
        throw new InvalidOperationException($"Error when bulk-indexing with Code: {result.Error.Code}, Description: {result.Error.Description}");
    }
}
