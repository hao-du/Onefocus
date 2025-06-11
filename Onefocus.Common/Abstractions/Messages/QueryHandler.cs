using Microsoft.AspNetCore.Http;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public abstract class QueryHandler<TRequest>(IHttpContextAccessor httpContextAccessor) : MediatorHandler(httpContextAccessor), IQueryHandler<TRequest> where TRequest : IQuery
{
    public virtual Task<Result> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => Result.Failure(CommonErrors.NotImplemented));
    }
}

public abstract class QueryHandler<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor) : MediatorHandler(httpContextAccessor), IQueryHandler<TRequest, TResponse> where TRequest : IQuery<TResponse>
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