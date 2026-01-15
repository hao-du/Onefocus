import TransactionItemResponse from "../TransactionItemResponse";

export default interface GetCashFlowByTransactionIdResponse {
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