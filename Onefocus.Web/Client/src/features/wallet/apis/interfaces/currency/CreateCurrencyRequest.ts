export default interface CreateCurrencyRequest {
    name: string;
    shortName: string;
    isDefault: boolean;
    description?: string;
}
