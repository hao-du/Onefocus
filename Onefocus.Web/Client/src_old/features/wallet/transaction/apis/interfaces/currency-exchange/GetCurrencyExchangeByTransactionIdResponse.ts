export default interface GetCurrencyExchangeByTransactionIdResponse {
    id: string;
    sourceAmount: number;
    sourceCurrencyId: string;
    targetAmount: number;
    targetCurrencyId: string;
    exchangeRate: number;
    transactedOn: Date;
    description?: string;
    isActive: boolean;
}