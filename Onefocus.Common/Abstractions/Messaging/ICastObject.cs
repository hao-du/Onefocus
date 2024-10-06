using MediatR;
using Onefocus.Common.Results;

namespace Onefocus.Common.Abstractions.Messaging;

public interface ICastObject<out TTargetRepsonse, in TSourceRepsonse>
{
    static abstract TTargetRepsonse Cast(TSourceRepsonse source);
}