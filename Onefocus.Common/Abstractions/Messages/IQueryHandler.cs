using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public interface IQueryHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : IQuery
{
}

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}