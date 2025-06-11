using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class BankAccountTransaction : ReadEntityBase
{
    public Guid BankAccountId { get; init; }
    public Guid TransactionId { get; init; }

    public BankAccount BankAccount { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;
}

