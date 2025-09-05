import PeerTransferTransactionDto from "./PeerTransferTransactionDto";

export default interface CreateBankAccountRequest {
    counterpartyId: string;
    status: number;
    type: number;
    description?: string;
    transactions: PeerTransferTransactionDto[];
}