using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Common.Repositories;

public abstract class BaseRepository<TRepository>(ILogger<TRepository> logger) : IBaseRepository
{
    private ILogger<TRepository> Logger { get; } = logger;

    protected async Task<Result> ExecuteAsync(Func<Task<Result>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, CommonErrors.InternalErrorMessage);
            return Result.Failure(ex.ToErrors());
        }
    }

    protected async Task<Result<Y>> ExecuteAsync<Y>(Func<Task<Result<Y>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, CommonErrors.InternalErrorMessage);
            return Result.Failure<Y>(ex.ToErrors());
        }
    }

    protected Result Execute(Func<Result> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, CommonErrors.InternalErrorMessage);
            return Result.Failure(ex.ToErrors());
        }
    }

    protected Result<Y> Execute<Y>(Func<Result<Y>> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, CommonErrors.InternalErrorMessage);
            return Result.Failure<Y>(ex.ToErrors());
        }
    }
}
