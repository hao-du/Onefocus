using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Bank;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class TransactionReadRepository(
    ILogger<BankReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<BankReadRepository>(logger, context), ITransactionReadRepository
{
    public async Task<Result<GetAllTransactionsResponseDto>> GetAllTransactionsAsync(GetAllTransactionsRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var transactions = await context.Transaction
                .Include(t => t.BankAccounts)
                    .ThenInclude(ba => ba.Bank)
                .Include(t => t.CashFlows)
                .Include(t => t.PeerTransfers)
                    .ThenInclude(u => u.TransferredUser)
                .Include(t => t.CurrencyExchanges)
                    .ThenInclude(ce => ce.TargetCurrency)
                    .ThenInclude(ce => ce.BaseCurrencyExchanges)
                .Where(t => t.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result.Success<GetAllTransactionsResponseDto>(new(transactions));
        });
    }
}