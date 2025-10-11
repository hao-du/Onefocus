namespace Onefocus.Wallet.Application.Contracts.Read.Currency;

public sealed record GetCurrencyByIdRequestDto(Guid Id, Guid UserId);