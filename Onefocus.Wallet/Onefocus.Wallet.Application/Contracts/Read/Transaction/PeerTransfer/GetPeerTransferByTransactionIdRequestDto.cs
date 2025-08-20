namespace Onefocus.Wallet.Application.Contracts.Read.Transaction.PeerTransfer;

public sealed record GetPeerTransferByTransactionIdRequestDto(Guid TransactionId, Guid UserId);