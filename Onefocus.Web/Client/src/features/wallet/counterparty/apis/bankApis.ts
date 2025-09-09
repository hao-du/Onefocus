import { ApiResponse, client } from '../../../../shared/hooks';
import { CreateCounterpartyRequest, CreateCounterpartyResponse, GetAllCounterpartiesResponse, GetCounterpartyByIdResponse, UpdateCounterpartyRequest } from './interfaces';

export const getAllCounterparties = async () => {
    const response = await client.get<ApiResponse<GetAllCounterpartiesResponse>>(`wallet/counterparty/all`);
    return response.data;
};

export const getCounterpartyById = async (id: string) => {
    const response = await client.get<ApiResponse<GetCounterpartyByIdResponse>>(`wallet/counterparty/${id}`);
    return response.data;
};

export const createCounterparty = async (request: CreateCounterpartyRequest) => {
    const response = await client.post<ApiResponse<CreateCounterpartyResponse>>(`wallet/counterparty/create`, request);
    return response.data;
};

export const updateCounterparty = async (request: UpdateCounterpartyRequest) => {
    const response = await client.put<ApiResponse>(`wallet/counterparty/update`, request);
    return response.data;
};