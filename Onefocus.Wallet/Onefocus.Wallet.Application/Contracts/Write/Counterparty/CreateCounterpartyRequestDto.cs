using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Counterparty
{
    public sealed record CreateCounterpartyRequestDto(Entity.Counterparty Counterparty);
}