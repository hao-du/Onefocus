import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {CreateCurrencyRequest, GetAllCurrenciesResponse, GetCurrencyByIdResponse} from './currency.interfaces';

export const getAllCurrencies = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<GetAllCurrenciesResponse>>(`wallet/currency/all`);
    return response.data;
};

export const getCurrencyById = async (client: AxiosInstance, id: string) => {
    const response = await client.get<ApiResponse<GetCurrencyByIdResponse>>(`wallet/currency/${id}}`);
    return response.data;
};

export const createCurrency = async (client: AxiosInstance, request: CreateCurrencyRequest) => {
    const response = await client.post<ApiResponse>(`wallet/currency/create`, request);
    return response.data;
};