import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {
    CreateCurrencyRequest, CreateCurrencyResponse,
    GetAllCurrenciesResponse,
    GetCurrencyByIdResponse,
    UpdateCurrencyRequest
} from './currency.interfaces';

export const getAllCurrencies = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<GetAllCurrenciesResponse>>(`wallet/currency/all`);
    return response.data;
};

export const getCurrencyById = async (client: AxiosInstance, id: string) => {
    const response = await client.get<ApiResponse<GetCurrencyByIdResponse>>(`wallet/currency/${id}`);
    return response.data;
};

export const createCurrency = async (client: AxiosInstance, request: CreateCurrencyRequest) => {
    const response = await client.post<ApiResponse<CreateCurrencyResponse>>(`wallet/currency/create`, request);
    return response.data;
};

export const updateCurrency = async (client: AxiosInstance, request: UpdateCurrencyRequest) => {
    const response = await client.put<ApiResponse>(`wallet/currency/update`, request);
    return response.data;
};