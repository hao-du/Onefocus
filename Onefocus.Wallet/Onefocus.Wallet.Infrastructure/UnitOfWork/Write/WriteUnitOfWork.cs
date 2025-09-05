using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions;
using Onefocus.Common.Results;
using Onefocus.Wallet.Application.Interfaces.Repositories.Write;
using Onefocus.Wallet.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Wallet.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Wallet.Infrastructure.UnitOfWork.Write;

public class WriteUnitOfWork(WalletWriteDbContext context
        , ILogger<WriteUnitOfWork> logger
        , IUserWriteRepository userRepository
        , IBankWriteRepository bankRepository
        , ICurrencyWriteRepository currencyRepository
        , ICounterpartyWriteRepository counterpartyWriteRepository
        , ITransactionWriteRepository transactionRepository
    ) : IWriteUnitOfWork
{
    private readonly WalletWriteDbContext _context = context;
    protected ILogger<WriteUnitOfWork> Logger { get; } = logger;
    public IUserWriteRepository User { get; } = userRepository;
    public IBankWriteRepository Bank { get; } = bankRepository;
    public ICurrencyWriteRepository Currency { get; } = currencyRepository;
    public ICounterpartyWriteRepository Counterparty { get; } = counterpartyWriteRepository;
    public ITransactionWriteRepository Transaction { get; } = transactionRepository;

    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var effectedRows = await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(effectedRows);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error in saving changes.");
            return Result.Failure<int>(ex.ToErrors());
        }
    }

    public async Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
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

    public async Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
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
                return Result.Failure<TRepsonse>(ex.ToErrors());
            }
        }
    }
}
