using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.CurrencyExchange;

public sealed record GetCurrencyExchangeByTransactionIdResponseDto(Entity.TransactionTypes.CurrencyExchange? CurrencyExchange);
