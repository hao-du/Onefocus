import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {
    CreateCashFlowRequest, CreateCashFlowResponse,
    GetAllTransactionsResponse,
    GetCashFlowByTransactionIdResponse
} from './transaction.interfaces';

export const getAllTransactions = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<GetAllTransactionsResponse>>(`wallet/transaction/all`);
    return response.data;
};

export const getCashFlowByTransactionId = async (client: AxiosInstance, transactionId: string) => {
    const response = await client.get<ApiResponse<GetCashFlowByTransactionIdResponse>>(`wallet/transaction/cashflow/${transactionId}`);
    return response.data; 
};

export const createCashFlow = async (client: AxiosInstance, request: CreateCashFlowRequest) => {
    const response = await client.post<ApiResponse<CreateCashFlowResponse>>(`wallet/transaction/cashflow/create`, request);
    return response.data;
};