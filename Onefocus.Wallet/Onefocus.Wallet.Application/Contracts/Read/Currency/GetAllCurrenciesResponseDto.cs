using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetAllCurrenciesResponseDto(List<Entity.Currency> Currencies);
