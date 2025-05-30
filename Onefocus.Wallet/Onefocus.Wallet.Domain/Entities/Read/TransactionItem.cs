using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Exceptions.Errors;
using Onefocus.Common.Results;

namespace Onefocus.Wallet.Domain.Entities.Read;

public class TransactionItem : ReadEntityBase
{
    public Guid TransactionId { get; init; }
    public string Name { get; init; } = default!;
    public decimal Amount { get; init; }

    public Transaction Transaction { get; init; } = default!;
}

