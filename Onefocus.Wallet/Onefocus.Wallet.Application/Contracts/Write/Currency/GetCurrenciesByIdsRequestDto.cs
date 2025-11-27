namespace Onefocus.Wallet.Application.Contracts.Write.Currency;

public sealed record GetCurrenciesByIdsRequestDto(IReadOnlyList<Guid> Ids);