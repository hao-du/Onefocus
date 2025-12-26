import { ApiResponse, client } from '../../../../shared/hooks';
import { CreateCurrencyRequest, CreateCurrencyResponse, GetAllCurrenciesResponse, GetCurrencyByIdResponse, UpdateCurrencyRequest } from './interfaces';
export const getAllCurrencies = async () => {
    const response = await client.get<ApiResponse<GetAllCurrenciesResponse>>(`wallet/currency/all`);
    return response.data;
};

export const getCurrencyById = async (id: string) => {
    const response = await client.get<ApiResponse<GetCurrencyByIdResponse>>(`wallet/currency/${id}`);
    return response.data;
};

export const createCurrency = async (request: CreateCurrencyRequest) => {
    const response = await client.post<ApiResponse<CreateCurrencyResponse>>(`wallet/currency/create`, request);
    return response.data;
};

export const updateCurrency = async (request: UpdateCurrencyRequest) => {
    const response = await client.put<ApiResponse>(`wallet/currency/update`, request);
    return response.data;
};