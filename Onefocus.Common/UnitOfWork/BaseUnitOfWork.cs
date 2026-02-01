using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions;
using Onefocus.Common.Results;

namespace Onefocus.Common.UnitOfWork;

public abstract class BaseUnitOfWork(
    DbContext context,
    ILogger<BaseUnitOfWork> logger
)
{
    public async Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var effectedRows = await context.SaveChangesAsync(cancellationToken);

            return Result.Success(effectedRows);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in saving changes.");
            return Result.Failure<int>(ex.ToErrors());
        }
    }

    public async Task<Result> WithTransactionAsync(Func<CancellationToken, Task<Result>> action, CancellationToken cancellationToken = default)
    {
        using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
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
                logger.LogError(ex, "Error in transaction");
                return Result.Failure(ex.ToErrors());
            }
        }
    }

    public async Task<Result<TRepsonse>> WithTransactionAsync<TRepsonse>(Func<CancellationToken, Task<Result<TRepsonse>>> action, CancellationToken cancellationToken = default)
    {
        using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
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
                logger.LogError(ex, "Error in transaction");
                return Result.Failure<TRepsonse>(ex.ToErrors());
            }
        }
    }
}
