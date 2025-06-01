using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Repositories.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

public class WriteUnitOfWork(WalletWriteDbContext context
        , ILogger<WriteUnitOfWork> logger
        , IUserWriteRepository userRepository
        , IBankWriteRepository bankRepository
        , ICurrencyWriteRepository currencyRepository
    ) : IWriteUnitOfWork
{
    private readonly WalletWriteDbContext _context = context;
    protected ILogger<WriteUnitOfWork> Logger { get; } = logger;
    public IUserWriteRepository User { get; } = userRepository;
    public IBankWriteRepository Bank { get; } = bankRepository;
    public ICurrencyWriteRepository Currency { get; } = currencyRepository;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await action(cancellationToken);
            if (result.IsSuccess)
            {
                await transaction.CommitAsync(cancellationToken);
            }

            return result;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in transaction");
            return Result.Failure(ex.ToErrors());
        }
    }
}
