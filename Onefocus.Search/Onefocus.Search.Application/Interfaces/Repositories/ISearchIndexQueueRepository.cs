using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts.SearchIndexQueue;

namespace Onefocus.Search.Application.Interfaces.Repositories;

public interface ISearchIndexQueueRepository : IBaseContextRepository
{
    Task<Result<GetSearchIndexQueuesResponseDto>> GetSearchIndexQueuesAsync(GetSearchIndexQueuesRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> AddSearchIndexQueueAsync(AddSearchIndexQueueRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> BulkUpdateActiveStatusAsync(BulkUpdateActiveStatusRequestDto request, CancellationToken cancellationToken = default);
}