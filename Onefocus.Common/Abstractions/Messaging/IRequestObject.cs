using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface IRequestObject<out TRequest> where TRequest : class
{
    TRequest ToRequestObject();
}