using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Messages.Read.Currency;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Read;

namespace Onefocus.Wallet.Domain.Repositories.Read;

public sealed class CurrencyReadRepository : BaseRepository<CurrencyReadRepository>, ICurrencyReadRepository
{
    private readonly WalletReadDbContext _context;

    public CurrencyReadRepository(
        ILogger<CurrencyReadRepository> logger
        , WalletReadDbContext context
    ) : base(logger)
    {
        _context = context;
    }

    public async Task<Result<GetAllCurrenciesResponseDto>> GetAllCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencies = await _context.Currency.AsNoTracking().ToListAsync(cancellationToken);
            return Result.Success(GetAllCurrenciesResponseDto.Cast(currencies));
        });
    }

    public async Task<Result<GetCurrencyByIdResponseDto>> GetCurrencyByIdAsync(GetCurrencyByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currency = await _context.Currency.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            return Result.Success(GetCurrencyByIdResponseDto.Cast(currency));
        });
    }

    public async Task<Result<GetCurrenciesBySpecificationResponseDto>> GetCurrencyBySpecificationAsync(GetCurrenciesBySpecificationRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var currencies = await _context.Currency.AsNoTracking().Where(request.Specification.ToExpression()).ToListAsync(cancellationToken);
            return Result.Success(GetCurrenciesBySpecificationResponseDto.Cast(currencies));
        });
    }
}