using Entity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.CurrencyExchange;

public sealed record GetCurrencyExchangeByIdResponseDto(Entity.CurrencyExchange? CurrencyExchange);