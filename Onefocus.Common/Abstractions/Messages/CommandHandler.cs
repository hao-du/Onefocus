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
        return Task.Run(() => Result.Failure<TResponse>(CommonErrors.NotImplemented));
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
}