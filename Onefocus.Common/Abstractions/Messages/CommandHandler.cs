using MediatR;
using Microsoft.AspNetCore.Http;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;
using System.Security.Claims;

namespace Onefocus.Common.Abstractions.Messages;

public abstract class CommandHandler<TRequest> : ICommandHandler<TRequest> where TRequest : ICommand
{
    protected readonly IHttpContextAccessor _httpContextAccessor;

    protected CommandHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public virtual async Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return await Task.Run(() => Result.Success());
    }

    protected Result<Guid> GetUserId()
    {
        if (!Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid userId))
        {
            return Result.Failure<Guid>(CommonErrors.UserClaimInvalid);
        }

        return userId;
    }
}