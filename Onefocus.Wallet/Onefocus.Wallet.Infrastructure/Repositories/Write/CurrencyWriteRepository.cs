using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Messages.Write;
using Onefocus.Wallet.Domain.Messages.Write.Currency;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Domain.Repositories.Write;

public sealed class CurrencyWriteRepository : BaseRepository<CurrencyWriteRepository>, ICurrencyWriteRepository
{
    private readonly WalletWriteDbContext _context;

    public CurrencyWriteRepository(
        ILogger<CurrencyWriteRepository> logger
        , WalletWriteDbContext context
    ) : base(logger)
    {
        _context = context;
    }

    public async Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currency = await _context.Currency.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success<GetCurrencyByIdResponseDto>(new(currency));
        });
    }

    public async Task<Result> AddCurrencyAsync(CreateCurrencyRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            await _context.AddAsync(request.Currency, cancellationToken);
            return Result.Success();
        });
    }

    public async Task<Result> BulkMarkDefaultFlag(BulkMarkDefaultFlagRequestDto request, CancellationToken cancellationToken = default)
    {
        var updatedOn = DateTimeOffset.Now;

        return await ExecuteAsync(async () =>
        {
            await _context.Currency.Where(c => !request.ExcludeIds.Contains(c.Id) && c.IsDefault == request.QueryValue).ExecuteUpdateAsync(setter => setter
                .SetProperty(c => c.IsDefault, request.UpdatingValue)
                .SetProperty(c => c.UpdatedBy, request.ActionedBy)
                .SetProperty(c => c.UpdatedOn, updatedOn)
            , cancellationToken);
            return Result.Success();
        });
    }
}