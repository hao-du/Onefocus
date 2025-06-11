using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class CurrencyExchangeTransaction : ReadEntityBase
{
    public Guid CurrencyExchangeId { get; init; }
    public Guid TransactionId { get; init; }
    public bool IsTarget { get; init; }

    public CurrencyExchange CurrencyExchange { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;
}

