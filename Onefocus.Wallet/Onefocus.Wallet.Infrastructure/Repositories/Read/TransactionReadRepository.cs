using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Transaction;
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
                .Include(t => t.BankAccountTransactions)
                    .ThenInclude(ba => ba.BankAccount)
                        .ThenInclude(b => b.Bank)
                .Include(t => t.CashFlows)
                .Include(t => t.PeerTransferTransactions)
                    .ThenInclude(pt => pt.PeerTransfer)
                        .ThenInclude(p => p.Counterparty)
                .Include(t => t.CurrencyExchangeTransactions)
                    .ThenInclude(et => et.CurrencyExchange)
                .Include(t => t.TransactionItems)
                .Include(t => t.Currency)
                .Where(t => t.OwnerUserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result.Success<GetAllTransactionsResponseDto>(new(transactions));
        });
    }

    public async Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var transaction = await context.Transaction
                .Include(t => t.CashFlows)
                .Include(t => t.TransactionItems)
                .Include(t => t.Currency)
                .Where(t => t.OwnerUserId == request.UserId && t.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            return Result.Success<GetCashFlowByIdResponseDto>(new(transaction));
        });
    }
}