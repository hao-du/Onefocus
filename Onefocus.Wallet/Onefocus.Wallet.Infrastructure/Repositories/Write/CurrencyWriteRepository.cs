using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Common.Utilities;
using Onefocus.Wallet.Application.Contracts.Write.Currency;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using System.Linq;

namespace Onefocus.Wallet.Infrastructure.Repositories.Write;

public sealed class CurrencyWriteRepository(
    ILogger<CurrencyWriteRepository> logger
        , WalletWriteDbContext context
    ) : BaseContextRepository<CurrencyWriteRepository>(logger, context), ICurrencyWriteRepository
{
    public async Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currency = await context.Currency.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetCurrencyByIdResponseDto>(new(currency));
        });
    }

    public async Task<Result<GetCurrenciesByIdsResponseDto>> GetCurrenciesByIdsAsync(GetCurrenciesByIdsRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencies = await context.Currency.Where(c => request.Ids.Contains(c.Id)).ToListAsync(cancellationToken);
            return Result.Success<GetCurrenciesByIdsResponseDto>(new(currencies));
        });
    }

    public async Task<Result> AddCurrencyAsync(CreateCurrencyRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await context.AddAsync(request.Currency, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result> BulkMarkDefaultFlag(BulkMarkDefaultFlagRequestDto request, CancellationToken cancellationToken = default)
    {
        var updatedOn = DateTimeExtensions.Now();

        return await ExecuteAsync(async () =>
        {
            await context.Currency.Where(c => !request.ExcludeIds.Contains(c.Id) && c.IsDefault == request.QueryValue).ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.IsDefault, request.UpdatingValue)
                .SetProperty(c => c.UpdatedBy, request.ActionedBy)
                .SetProperty(c => c.UpdatedOn, updatedOn)
            , cancellationToken);
            return Result.Success();
        });
    }
}