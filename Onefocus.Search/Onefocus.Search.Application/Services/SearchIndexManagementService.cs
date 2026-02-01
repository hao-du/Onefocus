using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts;
using Onefocus.Search.Application.Interfaces.Repositories;
using Onefocus.Search.Application.Interfaces.Services;

namespace Onefocus.Search.Application.Services;

public class SearchIndexManagementService(
    ISearchIndexQueueRepository searchIndexQueueWriteRepository,
    ISearchIndexService indexingService
) : ISearchIndexManagementService
{
    private const int BatchSize = 10;

    public async Task<Result> ExecuteSearchIndexAsync(CancellationToken cancellationToken = default)
    {
        var pendingSearchIndexItemsResult = await searchIndexQueueWriteRepository.GetSearchIndexQueuesAsync(new(BatchSize), cancellationToken);
        if (pendingSearchIndexItemsResult.IsFailure)
        {
            return pendingSearchIndexItemsResult;
        }

        var queueItems = pendingSearchIndexItemsResult.Value.Queues;
        if (queueItems.Count == 0)
        {
            return Result.Success();
        }

        var documents = queueItems.Select(q => new SearchIndexDocumentDto(
            IndexName: q.IndexName,
            DocumentId: q.DocumentId,
            Payload: q.Payload,
            VectorSearchTerms: q.VectorSearchTerms
        )).ToList();

        var indexResult = await indexingService.IndexEntities(documents, cancellationToken);
        if (indexResult.IsFailure) return indexResult;

        var bulkUpdateResult = await searchIndexQueueWriteRepository.BulkUpdateActiveStatusAsync(new([.. queueItems.Select(q => q.Id)]), cancellationToken);
        if (bulkUpdateResult.IsFailure) return bulkUpdateResult;

        return Result.Success();
    }
}
