import PeerTransferTransactionDto from "./PeerTransferTransactionDto";

export default interface GetPeerTransferByTransactionIdResponse {
    id: string;
    counterpartyId: string;
    status: number;
    type: number;
    description?: string;
    isActive: boolean;
    transactions: PeerTransferTransactionDto[];
}