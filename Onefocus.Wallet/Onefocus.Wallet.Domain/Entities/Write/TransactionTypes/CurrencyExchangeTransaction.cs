using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class CurrencyExchangeTransaction : WriteEntityBase
{
    public Guid CurrencyExchangeId { get; init; }
    public Guid TransactionId { get; init; }
    public bool IsTarget { get; init; }

    public CurrencyExchange CurrencyExchange { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;

    private CurrencyExchangeTransaction(Transaction transaction)
    {
        Transaction = transaction;
    }

    public static Result<CurrencyExchangeTransaction> Create(Transaction transaction)
    {
        return new CurrencyExchangeTransaction(transaction);
    }
}

