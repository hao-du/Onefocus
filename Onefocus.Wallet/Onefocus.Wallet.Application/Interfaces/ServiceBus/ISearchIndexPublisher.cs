using Onefocus.Common.Abstractions.ServiceBus.Membership;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Application.Interfaces.ServiceBus;

public interface ISearchIndexPublisher
{
    Task<Result> Publish(ISearchIndexMessage message, CancellationToken cancellationToken = default);
}
