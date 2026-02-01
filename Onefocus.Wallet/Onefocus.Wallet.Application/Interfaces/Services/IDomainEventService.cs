using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Application.Interfaces.Services;

internal interface IDomainEventService
{
    Task<Result> AddSearchIndexEvent(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken cancellationToken);
}
