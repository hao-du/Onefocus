export enum TransactionType {
    BankAccount = 0,
    PeerTransfer = 1,
    CurrencyExchange = 2,
    CashFlow = 3,
}

export interface Transaction {
    id: string;
    transactedOn: Date;
    amount: number;
    currencyName: string;
    description?: string;
    type: TransactionType;
    tags: string[];
}