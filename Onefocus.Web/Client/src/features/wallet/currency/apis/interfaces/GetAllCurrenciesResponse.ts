export default interface GetAllCurrenciesResponse {
    currencies : {
        id: string;
        name: string;
        shortName: string;
        isDefault: boolean;
        isActive: boolean;
        description?: string;
        actionedOn: Date;
        actionedBy: string;
    }[]
}