using Onefocus.Common.Abstractions.Domain;
using Onefocus.Common.Results;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class PeerTransfer : BaseTransaction
{
    public Guid TransferredUserId { get; init; }
    public PeerTransferStatus Status { get; init; }
    public PeerTransferType Type { get; init; }

    public User TransferredUser { get; init; } = default!;
}

