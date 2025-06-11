using Onefocus.Common.Abstractions.Domain;
using Onefocus.Wallet.Domain.Entities.Enums;
using Onefocus.Wallet.Domain.Entities.Read;
using Onefocus.Wallet.Domain.Entities.Read.TransactionTypes;

namespace Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

public sealed class PeerTransferTransaction : ReadEntityBase
{
    public Guid PeerTransferId { get; init; }
    public Guid TransactionId { get; init; }
    public Guid CounterpartyId { get; init; }
    public PeerTransferDirection Direction { get; init; }

    public PeerTransfer PeerTransfer { get; init; } = default!;
    public Transaction Transaction { get; init; } = default!;
    public Counterparty Counterparty { get; init; } = default!;

}

