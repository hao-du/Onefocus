export interface GetAllTransactionsResponse {
    transactions : TransactionResponse[];
}

export interface TransactionResponse {
    id: string;
    transactedOn: Date;
    amount: number;
    currencyName: string;
    description?: string;
    type: number;
    tags: string[];
}

export interface GetCashFlowByTransactionIdResponse {
    id: string;
    transactionId: string;
    transactedOn: Date;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
}