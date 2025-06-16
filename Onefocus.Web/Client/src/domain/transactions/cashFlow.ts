export interface CashFlow {
    id: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
}