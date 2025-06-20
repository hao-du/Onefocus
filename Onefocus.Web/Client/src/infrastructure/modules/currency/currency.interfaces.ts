export interface GetAllCurrenciesResponse {
    currencies : CurrencyResponse[];
}

export type GetCurrencyByIdResponse = CurrencyResponse;

export interface CreateCurrencyRequest {
    name: string;
    shortName: string;
    isDefault: boolean;
    description?: string;
}

export interface CreateCurrencyResponse{
    id: string;
}

export interface UpdateCurrencyRequest {
    id: string;
    name: string;
    shortName: string;
    isDefault: boolean;
    isActive: boolean;
    description?: string;
}

export interface CurrencyResponse {
    id: string;
    name: string;
    shortName: string;
    isDefault: boolean;
    isActive: boolean;
    description?: string;
    actionedOn: Date;
    actionedBy: string;
}