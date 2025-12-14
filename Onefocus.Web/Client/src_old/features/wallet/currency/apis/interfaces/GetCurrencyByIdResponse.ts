export default interface GetCurrencyByIdResponse {
    id: string;
    name: string;
    shortName: string;
    isDefault: boolean;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}