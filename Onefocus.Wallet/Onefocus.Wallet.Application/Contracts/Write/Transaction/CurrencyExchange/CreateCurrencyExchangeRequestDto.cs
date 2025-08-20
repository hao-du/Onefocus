using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.CurrencyExchange;

public sealed record CreateCurrencyExchangeRequestDto(Entity.TransactionTypes.CurrencyExchange CurrencyExchange);