using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Currency;

public sealed record GetAllCurrenciesResponseDto(List<Entity.Currency> Currencies);
