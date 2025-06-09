using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Repositories;
using Onefocus.Common.Results;

namespace Onefocus.Identity.Infrastructure.Databases.Repositories;

public abstract class BaseIdentityRepository<T>(
    ILogger<T> logger
    ) : BaseRepository<T>(logger), IBaseRepository
{
    protected Result<TResponse> GetIdentityErrorResult<TResponse>(IdentityResult identityResult)
    {
        var identityErrors = identityResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
        if (identityErrors.Count > 0)
        {
            return Result.Failure<TResponse>(identityErrors);
        }
        return Result.Failure<TResponse>(CommonErrors.Unknown);
    }

    protected Result<bool> GetIdentityErrorResult(IdentityResult identityResult)
    {
        var identityErrors = identityResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
        if (identityErrors.Count > 0)
        {
            return Result.Failure<bool>(identityErrors);
        }
        return Result.Failure<bool>(CommonErrors.Unknown);
    }
}