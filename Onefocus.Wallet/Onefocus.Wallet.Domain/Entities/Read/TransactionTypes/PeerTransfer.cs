using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class PeerTransfer : ReadEntityBase
{
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];

    public PeerTransferStatus Status { get; init; }
    public PeerTransferType Type { get; init; }

    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();
}
