import {ApiResponse, client} from '../../../../shared/hooks';
import { 
    GetAllTransactionsResponse, 
    CreateCashFlowRequest, 
    CreateCashFlowResponse, 
    GetCashFlowByTransactionIdResponse 
} from './interfaces';

export const getAllTransactions = async () => {
    const response = await client.get<ApiResponse<GetAllTransactionsResponse>>(`wallet/transaction/all`);
    return response.data;
};

export const getCashFlowByTransactionId = async (transactionId: string) => {
    const response = await client.get<ApiResponse<GetCashFlowByTransactionIdResponse>>(`wallet/transaction/cashflow/${transactionId}`);
    return response.data; 
};

export const createCashFlow = async (request: CreateCashFlowRequest) => {
    const response = await client.post<ApiResponse<CreateCashFlowResponse>>(`wallet/transaction/cashflow/create`, request);
    return response.data;
};