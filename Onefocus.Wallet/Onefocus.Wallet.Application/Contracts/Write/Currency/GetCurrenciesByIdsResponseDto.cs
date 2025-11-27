using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Currency;

public sealed record GetCurrenciesByIdsResponseDto(IReadOnlyList<Entity.Currency> Currencies);