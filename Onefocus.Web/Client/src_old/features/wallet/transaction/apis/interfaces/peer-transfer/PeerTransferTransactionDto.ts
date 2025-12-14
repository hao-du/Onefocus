export default interface PeerTransferTransactionDto {
    id?: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isInFlow: boolean;
    isActive: boolean;
}