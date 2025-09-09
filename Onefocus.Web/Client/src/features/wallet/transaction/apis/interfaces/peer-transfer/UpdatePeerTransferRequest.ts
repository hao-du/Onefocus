import PeerTransferTransactionDto from "./PeerTransferTransactionDto";

export default interface UpdatePeerTransferRequest {
    id: string;
    status: number;
    type: number;
    counterpartyId: string;
    isActive: boolean;
    description?: string;
    transferTransactions: PeerTransferTransactionDto[];
}