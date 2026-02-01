using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.ServiceBus.Search;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Application.Interfaces.ServiceBus;
using Onefocus.Wallet.Application.Interfaces.Services.Search;

namespace Onefocus.Wallet.Application.Services;

public class SearchIndexManagementService(
    ISearchIndexQueueReadRepository searchIndexQueueReadRepository,
    ISearchIndexPublisher searchIndexPublisher
) : ISearchIndexManagementService
{
    private const int BatchSize = 20;

    public async Task<Result> ExecuteSearchIndexAsync(CancellationToken cancellationToken = default)
    {
        var pendingSearchIndexItemsResult = await searchIndexQueueReadRepository.GetSearchIndexQueuesAsync(new(BatchSize));
        if (pendingSearchIndexItemsResult.IsFailure)
        {
            return pendingSearchIndexItemsResult;
        }

        var queueItems = pendingSearchIndexItemsResult.Value.Queues;
        if (queueItems.Count == 0)
        {
            return Result.Success();
        }

        var documents = queueItems.Select(q => new SearchIndexDocument(
            IndexName: q.IndexName,
            DocumentId: q.DocumentId,
            Payload: q.Payload,
            VectorSearchTerms: q.VectorSearchTerms
        )).ToList();
        await searchIndexPublisher.Publish(new SearchIndexMessage(documents), cancellationToken);

        return Result.Success();
    }
}
