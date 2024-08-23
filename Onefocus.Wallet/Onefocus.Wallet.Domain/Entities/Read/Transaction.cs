using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read;

public abstract class Transaction : WriteEntityBase
{
    private List<TransactionDetail> _transactionDetails = new List<TransactionDetail>();

    public decimal Amount { get; init; }
    public DateTimeOffset TransactedOn { get; init; }
    public Guid UserId { get; init; }
    public Guid CurrencyId { get; init; }

    public User User { get; init; } = default!;
    public Currency Currency { get; init; } = default!;
    public IReadOnlyCollection<TransactionDetail> TransactionDetails => _transactionDetails.AsReadOnly();
}

