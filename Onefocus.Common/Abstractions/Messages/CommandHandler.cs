using Microsoft.AspNetCore.Http;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System.Security.Claims;

namespace Onefocus.Common.Abstractions.Messages;

public abstract class CommandHandler<TRequest>(IHttpContextAccessor httpContextAccessor) : CommandHandler(httpContextAccessor), ICommandHandler<TRequest> where TRequest : ICommand
{
    public virtual Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => Result.Failure(CommonErrors.NotImplemented));
    }
}

public abstract class CommandHandler<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : CommandHandler(httpContextAccessor), ICommandHandler<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    public virtual Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => Failure(CommonErrors.NotImplemented));
    }

    protected Result<TResponse> Failure(Result failure)
    {
        return Result.Failure<TResponse>(failure.Errors);
    }
}

public abstract class CommandHandler(IHttpContextAccessor httpContextAccessor)
{
    protected Result<Guid> GetUserId()
    {
        if (!Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            return Result.Failure<Guid>(CommonErrors.UserClaimInvalid);
        }

        return userId;
    }

    protected string? GetCookie(string key)
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[key];
    }

    protected void AppendCookie(string key, string value, SameSiteMode mode = SameSiteMode.Unspecified, bool httpOnly = true)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, new CookieOptions
        {
            SameSite = mode,
            HttpOnly = httpOnly
        });
    }
}