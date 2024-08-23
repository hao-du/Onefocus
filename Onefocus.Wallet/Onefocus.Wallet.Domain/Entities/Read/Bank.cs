using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Read.Transactions;

namespace Onefocus.Wallet.Domain.Entities.Read;

public sealed class Bank : ReadEntityBase
{
    private List<BankingTransaction> _bankingTransactions = new List<BankingTransaction>();

    public string Name { get; init; } = default!;

    public IReadOnlyCollection<BankingTransaction> BankingTransactions => _bankingTransactions.AsReadOnly();
}