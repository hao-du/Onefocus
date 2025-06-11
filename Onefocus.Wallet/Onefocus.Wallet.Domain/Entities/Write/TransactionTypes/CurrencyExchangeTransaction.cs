using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CurrencyExchangeTransaction : ReadEntityBase
{
    public Guid CurrencyExchangeId { get; init; }
    public Guid TransactionId { get; init; }
    public Guid CurrencyId { get; init; }
    public CurrencyExchangeType ExchangeType { get; init; } = default!;

    public CurrencyExchange CurrencyExchange { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;
    public Currency Currency { get; init; } = default!;
}

