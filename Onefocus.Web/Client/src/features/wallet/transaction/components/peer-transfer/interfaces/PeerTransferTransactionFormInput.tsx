export default interface PeerTransferTransactionFormInput {
    id?: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isInFlow: boolean;
    isActive: boolean;
    rowId?: string
}