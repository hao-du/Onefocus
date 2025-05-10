using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Currency : ReadEntityBase
{
    private List<Transaction> _transactions = new List<Transaction>();
    private List<ExchangeTransaction> _exchangeTransactions = new List<ExchangeTransaction>();

    public string Name { get; init; } = default!;
    public string ShortName { get; init; } = default!;
    public bool DefaultFlag { get; init; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public IReadOnlyCollection<ExchangeTransaction> ExchangeTransactions => _exchangeTransactions.AsReadOnly();
}