import { TransactionItem } from "../transactionItem";

export interface CashFlow {
    id: string;
    transactionId: string;
    transactedOn: Date;
    amount: number;
    currencyId?: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
    transactionItems: TransactionItem[];
}