using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;

namespace Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

public sealed class PeerTransfer : ReadEntityBase
{
    private readonly List<PeerTransferTransaction> _peerTransferTransactions = [];

    public Guid CounterpartyId { get; init; }
    public PeerTransferStatus Status { get; init; }
    public PeerTransferType Type { get; init; }

    public Counterparty Counterparty { get; init; } = default!;

    public IReadOnlyCollection<PeerTransferTransaction> PeerTransferTransactions => _peerTransferTransactions.AsReadOnly();
}
