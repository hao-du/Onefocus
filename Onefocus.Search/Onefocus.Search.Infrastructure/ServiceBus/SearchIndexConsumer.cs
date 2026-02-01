using MassTransit;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.ServiceBus.Search;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Interfaces.UnitOfWork;
using Onefocus.Search.Domain.Entities;

namespace Onefocus.Search.Infrastructure.ServiceBus;

internal class SearchIndexConsumer(
    IUnitOfWork unitOfWork,
    ILogger<SearchIndexConsumer> logger
    ) : IConsumer<ISearchIndexMessage>
{
    public async Task Consume(ConsumeContext<ISearchIndexMessage> context)
    {
        var queues = context.Message.Documents.Select(entity => SearchIndexQueue.Create(
            documentId: entity.DocumentId,
            indexName: entity.IndexName,
            payload: entity.Payload,
            vectorSearchTerms: entity.VectorSearchTerms
        ).Value).ToList();

        var addQueueResult = await unitOfWork.SearchIndexQueue.AddSearchIndexQueueAsync(new(queues));
        if (addQueueResult.IsFailure)
        {
            LogError(addQueueResult);
            return;
        }

        var saveResult = await unitOfWork.SaveChangesAsync();
        if (saveResult.IsFailure)
        {
            LogError(saveResult);
            return;
        }
    }

    private void LogError(Result result)
    {
        foreach (var error in result.Errors)
        {
            logger.LogError("Error when bulk-indexing with Code: {Code}, Description: {Description}", error.Code, error.Description);
        }
    }
}
