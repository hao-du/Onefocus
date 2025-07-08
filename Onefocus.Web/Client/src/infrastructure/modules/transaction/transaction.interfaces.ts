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

export interface TransactionItem {
    name: string;
    amount: number;
    description?: string;
}

export interface CreateCashFlowRequest {
    amount: number;
    transactedOn: Date;
    isIncome: boolean;
    currencyId: string;
    description?: string;
    transactionItems: TransactionItem[];
}
export interface CreateCashFlowResponse {
    id: string;
}