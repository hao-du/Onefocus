using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class BankAccountTransaction : WriteEntityBase
{
    public Guid BankAccountId { get; private set; }
    public Guid TransactionId { get; private set; }

    public BankAccount BankAccount { get; private set; } = default!;
    public Transaction Transaction { get; private set; } = default!;

    private BankAccountTransaction(Transaction transaction)
    {
        Transaction = transaction;
    }

    public static BankAccountTransaction Create(Transaction transaction)
    {
        return new BankAccountTransaction(transaction);
    }
}

