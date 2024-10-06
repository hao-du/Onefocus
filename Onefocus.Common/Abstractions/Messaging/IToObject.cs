using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface IToObject<out TRequest> where TRequest : class
{
    TRequest ToObject();
}

public interface IToObject<out TRequest, in TParams> where TRequest : class
{
    TRequest ToObject(TParams? parameters = default);
}