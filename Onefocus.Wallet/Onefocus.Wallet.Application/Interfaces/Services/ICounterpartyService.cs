using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Interfaces.Services;

internal interface ICounterpartyService
{
    Task PublishEvents(Entity.Counterparty counterparty, CancellationToken cancellationToken);
}
