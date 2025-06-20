﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Bank;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.BankAccount;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class TransactionWriteRepository(
    ILogger<TransactionWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<TransactionWriteRepository>(logger, context), ITransactionWriteRepository
{
    public async Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var transaction = await context.Transaction
                .Include(t => t.CashFlows)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetCashFlowByIdResponseDto>(new(transaction));
        });
    }

    public async Task<Result> AddCashFlowAsync(CreateCashFlowRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Transaction, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result<GetBankAccountByIdResponseDto>> GetBankAccountByIdAsync(GetBankAccountByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var transaction = await context.BankAccount
                .Include(t => t.BankAccountTransactions)
                    .ThenInclude(bat => bat.Transaction)
                .FirstOrDefaultAsync(c => c.Bank == request.Id, cancellationToken);
            return Result.Success<GetBankAccountByIdResponseDto>(new(transaction));
        });
    }

    public async Task<Result> AddBankAccountAsync(CreateBankAccountRequestDto request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}