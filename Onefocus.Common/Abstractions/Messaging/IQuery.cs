using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}