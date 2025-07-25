export interface CashFlowFormInput {
    id?: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
    transactionItems: {
        id?: string;
        name: string;
        amount: number;
        isActive: boolean;
        description?: string;
    }[];
}