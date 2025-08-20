using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class CurrencyExchange : ReadEntityBase
{
    private readonly List<CurrencyExchangeTransaction> _currencyExchangeTransactions = [];

    public decimal ExchangeRate { get; init; }

    public IReadOnlyCollection<CurrencyExchangeTransaction> CurrencyExchangeTransactions => _currencyExchangeTransactions.AsReadOnly();

    public Transaction GetSource()
    {
        return _currencyExchangeTransactions
            .Where(t => !t.IsTarget)
            .Select(t => t.Transaction)
            .First();
    }

    public Transaction GetTarget()
    {
        return _currencyExchangeTransactions
            .Where(t => t.IsTarget)
            .Select(t => t.Transaction)
            .First();
    }

    public DateTimeOffset GetTransactedOn()
    {
        return _currencyExchangeTransactions.First().Transaction.TransactedOn;
    }
}