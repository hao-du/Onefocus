using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;

namespace Onefocus.Membership.Application.Interfaces.ServiceBus;

public interface IUserSyncedPublisher
{
    Task<Result> Publish(ISyncUserMessage message, CancellationToken cancellationToken = default);
}
