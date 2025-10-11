using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Contracts.Read.Currency;
using Onefocus.Wallet.Application.Interfaces.Repositories.Read;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Infrastructure.Repositories.Read;

public sealed class CurrencyReadRepository(ILogger<CurrencyReadRepository> logger, WalletReadDbContext context) : BaseContextRepository<CurrencyReadRepository>(logger, context), ICurrencyReadRepository
{
    public async Task<Result<GetAllCurrenciesResponseDto>> GetAllCurrenciesAsync(GetAllCurrenciesRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencies = await context.Currency.Where(c => c.OwnerUserId == request.UserId).ToListAsync(cancellationToken);
            return Result.Success<GetAllCurrenciesResponseDto>(new(currencies));
        });
    }

    public async Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currency = await context.Currency.FirstOrDefaultAsync(c => c.Id == request.Id && c.OwnerUserId == request.UserId, cancellationToken);
            return Result.Success<GetCurrencyByIdResponseDto>(new(currency));
        });
    }
}
