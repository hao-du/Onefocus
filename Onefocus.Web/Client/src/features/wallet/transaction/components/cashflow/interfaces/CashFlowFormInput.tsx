import TransactionItemFormInput from "./TransactionItemFormInput";

export default interface CashFlowFormInput {
    id?: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
    transactionItems: TransactionItemFormInput[];
}