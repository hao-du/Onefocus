import TransactionItemResponse from "../TransactionItemResponse";

export default interface UpdateCashFlowRequest {
    id: string;
    amount: number;
    transactedOn: Date;
    isIncome: boolean;
    isActive: boolean;
    currencyId: string;
    description?: string;
    transactionItems: TransactionItemResponse[];
}