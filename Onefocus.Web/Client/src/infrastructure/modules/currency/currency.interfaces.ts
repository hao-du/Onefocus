export interface GetAllCurrenciesResponse {
    currencies : CurrencyResponse[];
}

export type GetCurrencyByIdResponse = CurrencyResponse;

export interface CreateCurrencyRequest {
    name: string;
    shortName: string;
    defaultFlag: boolean;
    description?: string;
}

export interface CurrencyResponse {
    id: number;
    name: string;
    shortName: string;
    defaultFlag: boolean;
    activeFlag: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}