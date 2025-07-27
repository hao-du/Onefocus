export default interface CreateCashFlowRequest {
    amount: number;
    transactedOn: Date;
    isIncome: boolean;
    currencyId: string;
    description?: string;
    transactionItems: {
        name: string;
        amount: number;
        description?: string;
    }[];
}