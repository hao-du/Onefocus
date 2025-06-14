using Onefocus.Common.Abstractions.Domain;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class PeerTransferTransaction : ReadEntityBase
{
    public Guid PeerTransferId { get; init; }
    public Guid TransactionId { get; init; }
    public bool IsInFlow { get; init; }

    public PeerTransfer PeerTransfer { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;
}

