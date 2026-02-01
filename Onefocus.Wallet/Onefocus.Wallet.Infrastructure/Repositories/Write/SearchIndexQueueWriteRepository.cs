using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.SearchIndexQueue;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class SearchIndexQueueWriteRepository(
    ILogger<BankWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<BankWriteRepository>(logger, context), ISearchIndexQueueWriteRepository
{
    public async Task<Result> AddSearchIndexQueueAsync(AddSearchIndexQueueRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddRangeAsync(request.domainEvents, cancellationToken);
            return Result.Success();
        });
    }
}