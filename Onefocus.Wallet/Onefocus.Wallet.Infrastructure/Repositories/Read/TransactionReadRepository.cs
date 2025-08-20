using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Transaction;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.BankAccount;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.CashFlow;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.CurrencyExchange;
using Onefocus.Wallet.Application.Contracts.Read.Transaction.PeerTransfer;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class TransactionReadRepository(
    ILogger<TransactionReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<TransactionReadRepository>(logger, context), ITransactionReadRepository
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

    public async Task<Result<GetCashFlowByTransactionIdResponseDto>> GetCashFlowByTransactionIdAsync(GetCashFlowByTransactionIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var cashFlow = await context.CashFlow
                .Include(cf => cf.Transaction)
                .ThenInclude(t => t.TransactionItems)
                .Where(cf => cf.Transaction.OwnerUserId == request.UserId && cf.Id == request.TransactionId)
                .SingleOrDefaultAsync(cancellationToken);

            return Result.Success<GetCashFlowByTransactionIdResponseDto>(new(cashFlow));
        });
    }

    public async Task<Result<GetBankAccountByTransactionIdResponseDto>> GetBankAccountByTransactionIdAsync(GetBankAccountByTransactionIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var bankAccountId = await context.Transaction
                .Where(c => c.Id == request.TransactionId)
                .SelectMany(c => c.BankAccountTransactions.Select(bat => bat.BankAccountId))
                .FirstOrDefaultAsync(cancellationToken);

            var bankAccount = await context.BankAccount
                .Include(ba => ba.BankAccountTransactions)
                .ThenInclude(bat => bat.Transaction)
                .Where(ba => ba.OwnerUserId == request.UserId && ba.Id == bankAccountId)
                .SingleOrDefaultAsync(cancellationToken);

            return Result.Success<GetBankAccountByTransactionIdResponseDto>(new(bankAccount));
        });
    }

    public async Task<Result<GetPeerTransferByTransactionIdResponseDto>> GetPeerTransferByTransactionIdAsync(GetPeerTransferByTransactionIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var peerTransferId = await context.Transaction
                .Where(t => t.Id == request.TransactionId && t.OwnerUserId == request.UserId)
                .SelectMany(t => t.PeerTransferTransactions.Select(ptt => ptt.PeerTransferId))
                .FirstOrDefaultAsync(cancellationToken);

            var peerTransfer = await context.PeerTransfer
                .Include(pt => pt.PeerTransferTransactions)
                .ThenInclude(ptt => ptt.Transaction)
                .Where(pt => pt.Id == peerTransferId)
                .SingleOrDefaultAsync(cancellationToken);

            return Result.Success<GetPeerTransferByTransactionIdResponseDto>(new(peerTransfer));
        });
    }

    public async Task<Result<GetCurrencyExchangeByTransactionIdResponseDto>> GetCurrencyExchangeByTransactionIdAsync(GetCurrencyExchangeByTransactionIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencyExchangeId = await context.Transaction
                .Where(t => t.Id == request.TransactionId && t.OwnerUserId == request.UserId)
                .SelectMany(t => t.CurrencyExchangeTransactions.Select(ptt => ptt.CurrencyExchangeId))
                .FirstOrDefaultAsync(cancellationToken);

            var currencyExchange = await context.CurrencyExchange
                .Include(pt => pt.CurrencyExchangeTransactions)
                .ThenInclude(ptt => ptt.Transaction)
                .Where(pt => pt.Id == currencyExchangeId)
                .SingleOrDefaultAsync(cancellationToken);

            return Result.Success<GetCurrencyExchangeByTransactionIdResponseDto>(new(currencyExchange));
        });
    }
}