using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Search.Application.Contracts.SearchIndexQueue;
using Onefocus.Search.Application.Interfaces.Repositories;
using Onefocus.Search.Infrastructure.Databases.DbContexts;

namespace Onefocus.Search.Infrastructure.Repositories;

public sealed class SearchIndexQueueRepository(
    ILogger<SearchIndexQueueRepository> logger
        , SearchDbContext context
    ) : BaseContextRepository<SearchIndexQueueRepository>(logger, context), ISearchIndexQueueRepository
{
    public async Task<Result<GetSearchIndexQueuesResponseDto>> GetSearchIndexQueuesAsync(GetSearchIndexQueuesRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var queues = await context.SearchIndexQueue.OrderBy(c => c.CreatedOn).Take(request.BatchSize).ToListAsync(cancellationToken);
            return Result.Success<GetSearchIndexQueuesResponseDto>(new(queues));
        });
    }

    public async Task<Result> AddSearchIndexQueueAsync(AddSearchIndexQueueRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddRangeAsync(request.searchIndexQueues, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result> BulkUpdateActiveStatusAsync(BulkUpdateActiveStatusRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.SearchIndexQueue
            .Where(s => request.Ids.Contains(s.Id))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.IsActive, s => false)
            , cancellationToken);
            return Result.Success();
        });
    }
}