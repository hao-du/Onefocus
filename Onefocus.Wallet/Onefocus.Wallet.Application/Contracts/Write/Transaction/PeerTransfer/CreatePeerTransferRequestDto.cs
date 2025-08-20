using Entity = Onefocus.Wallet.Domain.Entities.Write;

namespace Onefocus.Wallet.Application.Contracts.Write.Transaction.PeerTransfer;

public sealed record CreatePeerTransferRequestDto(Entity.TransactionTypes.PeerTransfer PeerTransfer);