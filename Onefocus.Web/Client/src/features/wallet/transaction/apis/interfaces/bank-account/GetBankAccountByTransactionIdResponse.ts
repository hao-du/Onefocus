import TransactionItemResponse from "../TransactionItemResponse";

export default interface GetBankAccountByTransactionIdResponse {
    id: string;
    transactionId: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
    transactionItems: TransactionItemResponse[];
}