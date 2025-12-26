export default interface UpdateCurrencyRequest {
    id: string;
    name: string;
    shortName: string;
    isDefault: boolean;
    isActive: boolean;
    description?: string;
}