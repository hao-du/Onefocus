using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public abstract class CommandHandler<TRequest>(IHttpContextAccessor httpContextAccessor, ILogger logger) : MediatorHandler(httpContextAccessor, logger), ICommandHandler<TRequest> where TRequest : ICommand
{
    public virtual Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => Result.Failure(CommonErrors.NotImplemented));
    }
}

public abstract class CommandHandler<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor, ILogger logger) : MediatorHandler(httpContextAccessor, logger), ICommandHandler<TRequest, TResponse> where TRequest : ICommand<TResponse>
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