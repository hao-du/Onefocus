namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.CurrencyExchange;

public sealed record GetCurrencyExchangeByTransactionIdRequestDto(Guid TransactionId, Guid UserId);