using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Domain.Messages.Write.Currency;

public sealed record GetCurrencyByIdResponseDto(Entity.Currency? Currency);