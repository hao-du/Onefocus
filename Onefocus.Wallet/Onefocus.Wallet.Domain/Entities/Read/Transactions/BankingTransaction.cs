using Onefocus.Wallet.Domain.Entities.Read.ObjectValues;

namespace Onefocus.Wallet.Domain.Entities.Read.Transactions;

public sealed class BankingTransaction : Transaction
{
    public BankAccount BankAccount { get; init; } = default!;
    public Guid BankId { get; init; }

    public Bank Bank { get; init; } = default!;
}

