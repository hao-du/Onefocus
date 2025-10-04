using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions;
using Onefocus.Common.Results;
using Onefocus.Home.Application.Interfaces.Repositories.Write;
using Onefocus.Home.Application.Interfaces.UnitOfWork.Write;
using Onefocus.Home.Infrastructure.Databases.DbContexts.Write;

namespace Onefocus.Home.Infrastructure.UnitOfWork.Write;

public class WriteUnitOfWork(HomeWriteDbContext context
        , ILogger<WriteUnitOfWork> logger
        , IUserWriteRepository userRepository
        , ISettingsWriteRepository settingRepository
    ) : IWriteUnitOfWork
{
    private readonly HomeWriteDbContext _context = context;
    protected ILogger<WriteUnitOfWork> Logger { get; } = logger;
    public IUserWriteRepository User { get; } = userRepository;
    public ISettingsWriteRepository Settings { get; } = settingRepository;

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
