import PeerTransferTransactionDto from "./PeerTransferTransactionDto";

export default interface CreatePeerTransferRequest {
    status: number;
    type: number;
    counterpartyId: string;
    description?: string;
    transferTransactions: PeerTransferTransactionDto[];
}