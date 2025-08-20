using Entity = Onefocus.Wallet.Domain.Entities.Read;

namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.PeerTransfer;

public sealed record GetPeerTransferByTransactionIdResponseDto(Entity.TransactionTypes.PeerTransfer? PeerTransfer);
