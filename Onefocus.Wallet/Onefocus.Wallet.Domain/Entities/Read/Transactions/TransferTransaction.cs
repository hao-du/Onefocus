namespace Onefocus.Wallet.Domain.Entities.Read.Transactions;

public sealed class TransferTransaction : Transaction
{
    public Guid TransferredUserId { get; init; }

    public User TransferredUser { get; init; } = default!;
}

