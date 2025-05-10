using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Messages.Write;
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

    public async Task<Result> CreateCurrencyAsync(CreateCurrencyRequestDto request, CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(async () =>
        {
            var result = request.ToObject();
            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _context.AddAsync(result.Value, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        });
    }
}