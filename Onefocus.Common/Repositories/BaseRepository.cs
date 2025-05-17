using Microsoft.Extensions.Logging;
using Onefocus.Common.Abstractions.Messages;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Common.Repositories;

public abstract class BaseRepository<T>: IBaseRepository where T : class
{
    protected ILogger<T> _logger { get; }

    protected BaseRepository(ILogger<T> logger)
    {
        _logger = logger;
    }

    protected async Task<Result> ExecuteAsync(Func<Task<Result>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
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
            _logger.LogError(ex, ex.Message);
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
            _logger.LogError(ex, ex.Message);
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
            _logger.LogError(ex, ex.Message);
            return Result.Failure<Y>(ex.ToErrors());
        }
    }
}
