using Entity = Onefocus.Wallet.Domain.Entities.Write.TransactionTypes;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.PeerTransfer;

public sealed record GetPeerTransferByIdResponseDto(Entity.PeerTransfer? PeerTransfer);