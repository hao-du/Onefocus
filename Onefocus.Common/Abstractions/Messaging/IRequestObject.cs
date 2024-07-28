using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface IRequestObject<TRequest> where TRequest : class
{
    TRequest ToRequestObject();
}