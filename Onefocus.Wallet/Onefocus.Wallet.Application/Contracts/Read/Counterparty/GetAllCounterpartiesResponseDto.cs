using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Counterparty;

public sealed record GetAllCounterpartiesResponseDto(List<Entity.Counterparty> Counterparties);
