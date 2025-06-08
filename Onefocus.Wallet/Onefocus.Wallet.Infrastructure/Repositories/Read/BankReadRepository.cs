using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Domain.Messages.Read.Bank;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class BankReadRepository(
    ILogger<BankReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<BankReadRepository>(logger, context), IBankReadRepository
{
    public async Task<Result<GetAllBanksResponseDto>> GetAllBanksAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var banks = await context.Bank.ToListAsync(cancellationToken);
            return Result.Success<GetAllBanksResponseDto>(new(banks));
        });
    }

    public async Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var bank = await context.Bank.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetBankByIdResponseDto>(new(bank));
        });
    }
}