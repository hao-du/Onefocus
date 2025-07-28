import TransactionItemResponse from "./TransactionItemResponse";

export default interface CreateCashFlowRequest {
    amount: number;
    transactedOn: Date;
    isIncome: boolean;
    currencyId: string;
    description?: string;
    transactionItems: TransactionItemResponse[];
}