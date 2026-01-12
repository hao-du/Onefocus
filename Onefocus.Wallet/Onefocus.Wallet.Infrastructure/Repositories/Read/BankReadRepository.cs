using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Bank;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class BankReadRepository(
    ILogger<BankReadRepository> logger
        , WalletReadDbContext context
    ) : BaseContextRepository<BankReadRepository>(logger, context), IBankReadRepository
{
    public async Task<Result<GetBanksResponseDto>> GetBanksAsync(GetBanksRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var banks = await context.Bank.Where(c =>
                c.OwnerUserId == request.UserId
                && (string.IsNullOrEmpty(request.Name) || c.Name.Contains(request.Name))
                && (string.IsNullOrEmpty(request.Description) || c.Description.Contains(request.Description))
            ).ToListAsync(cancellationToken);
            return Result.Success<GetBanksResponseDto>(new(banks));
        });
    }

    public async Task<Result<GetBankByIdResponseDto>> GetBankByIdAsync(GetBankByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var bank = await context.Bank.FirstOrDefaultAsync(c => c.Id == request.Id && c.OwnerUserId == request.UserId, cancellationToken);
            return Result.Success<GetBankByIdResponseDto>(new(bank));
        });
    }
}