using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.SearchIndexQueue;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class SearchIndexQueueReadRepository(
    ILogger<SearchIndexQueueReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<SearchIndexQueueReadRepository>(logger, context), ISearchIndexQueueReadRepository
{
    public async Task<Result<GetSearchIndexQueuesResponseDto>> GetSearchIndexQueuesAsync(GetSearchIndexQueuesRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var queues = await context.SearchIndexQueue.OrderBy(c => c.CreatedOn).Take(request.BatchSize).ToListAsync(cancellationToken);
            return Result.Success<GetSearchIndexQueuesResponseDto>(new(queues));
        });
    }
}