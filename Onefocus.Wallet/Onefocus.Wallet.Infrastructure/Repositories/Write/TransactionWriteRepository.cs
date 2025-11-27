using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CurrencyExchange;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.PeerTransfer;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class TransactionWriteRepository(
    ILogger<TransactionWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<TransactionWriteRepository>(logger, context), ITransactionWriteRepository
{
    public async Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var cashFlow = await context.CashFlow
                .Include(cf => cf.Transaction)
                .ThenInclude(t => t.TransactionItems)
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetCashFlowByIdResponseDto>(new(cashFlow));
        });
    }

    public async Task<Result<GetBankAccountByIdResponseDto>> GetBankAccountByIdAsync(GetBankAccountByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var bankAccount = await context.BankAccount
                .Include(ba => ba.BankAccountTransactions)
                .ThenInclude(bat => bat.Transaction)
                .FirstOrDefaultAsync(ba => ba.Id == request.Id, cancellationToken);
            return Result.Success<GetBankAccountByIdResponseDto>(new(bankAccount));
        });
    }

    public async Task<Result<GetPeerTransferByIdResponseDto>> GetPeerTransferByIdAsync(GetPeerTransferByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var peerTransfer = await context.PeerTransfer
                .Include(pt => pt.PeerTransferTransactions)
                .ThenInclude(ptt => ptt.Transaction)
                .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);
            return Result.Success<GetPeerTransferByIdResponseDto>(new(peerTransfer));
        });
    }

    public async Task<Result<GetCurrencyExchangeByIdResponseDto>> GetCurrencyExchangeByIdAsync(GetCurrencyExchangeByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencyExchange = await context.CurrencyExchange
                .Include(pt => pt.CurrencyExchangeTransactions)
                .ThenInclude(ptt => ptt.Transaction)
                .FirstOrDefaultAsync(pt => pt.Id == request.Id, cancellationToken);
            return Result.Success<GetCurrencyExchangeByIdResponseDto>(new(currencyExchange));
        });
    }

    public async Task<Result> AddCashFlowAsync(CreateCashFlowRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.CashFlow, cancellationToken);

            if(request.CashFlow.Transaction != null && request.CashFlow.Transaction.CurrencyId != Guid.Empty)
            {
                await context.Entry(request.CashFlow).Reference(c => c.Transaction.Currency).LoadAsync();
            }

            return Result.Success();
        });
    }

    public async Task<Result> AddBankAccountAsync(CreateBankAccountRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.BankAccount, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result> AddPeerTransferAsync(CreatePeerTransferRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.PeerTransfer, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result> AddCurrencyExchangeAsync(CreateCurrencyExchangeRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.CurrencyExchange, cancellationToken);
            return Result.Success();
        });
    }
}