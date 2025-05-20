using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Domain.Messages.Read.Currency;

public sealed record GetCurrencyByIdResponseDto(Entity.Currency? Currency);