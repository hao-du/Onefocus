import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {
    CreateBankRequest,
    GetAllBanksResponse,
    GetBankByIdResponse,
    UpdateBankRequest
} from './bank.interfaces';

export const getAllBanks = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<GetAllBanksResponse>>(`wallet/bank/all`);
    return response.data;
};

export const getBankById = async (client: AxiosInstance, id: string) => {
    const response = await client.get<ApiResponse<GetBankByIdResponse>>(`wallet/bank/${id}}`);
    return response.data;
};

export const createBank = async (client: AxiosInstance, request: CreateBankRequest) => {
    const response = await client.post<ApiResponse>(`wallet/bank/create`, request);
    return response.data;
};

export const updateBank = async (client: AxiosInstance, request: UpdateBankRequest) => {
    const response = await client.put<ApiResponse>(`wallet/bank/update`, request);
    return response.data;
};