using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface IResponseObject<out TTargetRepsonse, in TSourceRepsonse>
{
    static abstract TTargetRepsonse Create(TSourceRepsonse source);
}