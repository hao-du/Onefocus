import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {GetAllTransactionsResponse, GetCashFlowByIdResponse} from './transaction.interfaces';

export const getAllTransactions = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<GetAllTransactionsResponse>>(`wallet/transaction/all`);
    return response.data;
};

export const getCashFlowById = async (client: AxiosInstance, id: string) => {
    const response = await client.get<ApiResponse<GetCashFlowByIdResponse>>(`wallet/transaction/cashflow/${id}}`);
    return response.data;
};