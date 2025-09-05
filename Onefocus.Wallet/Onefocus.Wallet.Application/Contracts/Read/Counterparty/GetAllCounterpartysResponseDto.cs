using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Counterparty;

public sealed record GetAllCounterpartysResponseDto(List<Entity.Counterparty> Counterparties);
