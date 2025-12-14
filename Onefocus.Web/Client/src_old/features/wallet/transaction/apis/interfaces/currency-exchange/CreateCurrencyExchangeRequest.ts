export default interface CreateCurrencyExchangeRequest {
    sourceAmount: number;
    sourceCurrencyId: string;
    targetAmount: number;
    targetCurrencyId: string;
    exchangeRate: number;
    transactedOn: Date;
    description?: string;
}