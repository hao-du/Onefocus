using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messages;

public interface ICommandHandler<in TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : ICommand
{
}

public interface ICommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : ICommand<TResponse>
{
}