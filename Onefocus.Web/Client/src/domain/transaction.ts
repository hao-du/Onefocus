export interface Transaction {
    id: string;
    transactedOn: Date;
    amount: number;
    currencyName: string;
    description?: string;
    type: number;
    tags: string[];
}