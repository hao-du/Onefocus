using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Write.Transaction.CashFlow;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class TransactionWriteRepository(
    ILogger<TransactionWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<TransactionWriteRepository>(logger, context), ITransactionWriteRepository
{
    public async Task<Result<GetCashFlowByIdResponseDto>> GetCashFlowByTransactionIdAsync(GetCashFlowByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var cashFlow = await context.CashFlow
                .Include(t => t.Transaction)
                .FirstOrDefaultAsync(c => c.TransactionId == request.TransactionId, cancellationToken);
            return Result.Success<GetCashFlowByIdResponseDto>(new(cashFlow));
        });
    }

    public async Task<Result> AddCashFlowAsync(CreateCashFlowRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.CashFlow, cancellationToken);
            return Result.Success();
        });
    }
}