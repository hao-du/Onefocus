using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Currency;

public sealed record CreateCurrencyRequestDto(Entity.Currency Currency);
