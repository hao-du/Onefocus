using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class TransactionDetail : WriteEntityBase
{
    public decimal Amount { get; init; }
    public DateTimeOffset TransactedOn { get; init; }
    public Enums.Action Action { get; init; }
    public Guid TransactionId { get; init; }

    public Transaction Transaction { get; init; } = default!;
}

