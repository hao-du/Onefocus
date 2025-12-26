import PeerTransferTransactionFormInput from "./PeerTransferTransactionFormInput";

export default interface PeerTransferFormInput {
    id?: string;
    status: number;
    type: number;
    counterpartyId: string;
    isActive: boolean;
    description?: string;
    transferTransactions: PeerTransferTransactionFormInput[];
}