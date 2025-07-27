import { ApiResponse, client } from '../../../../shared/hooks';
import {
    CreateBankRequest,
    CreateBankResponse,
    GetAllBanksResponse,
    GetBankByIdResponse,
    UpdateBankRequest
} from './interfaces';

export const getAllBanks = async () => {
    const response = await client.get<ApiResponse<GetAllBanksResponse>>(`wallet/bank/all`);
    return response.data;
};

export const getBankById = async (id: string) => {
    const response = await client.get<ApiResponse<GetBankByIdResponse>>(`wallet/bank/${id}`);
    return response.data;
};

export const createBank = async (request: CreateBankRequest) => {
    const response = await client.post<ApiResponse<CreateBankResponse>>(`wallet/bank/create`, request);
    return response.data;
};

export const updateBank = async (request: UpdateBankRequest) => {
    const response = await client.put<ApiResponse>(`wallet/bank/update`, request);
    return response.data;
};