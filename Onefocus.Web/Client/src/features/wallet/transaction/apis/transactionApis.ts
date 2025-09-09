import { ApiResponse, client } from '../../../../shared/hooks';
import {
    GetAllTransactionsResponse,
    CreateCashFlowRequest,
    CreateCashFlowResponse,
    GetCashFlowByTransactionIdResponse,
    UpdateCashFlowRequest,
    GetBankAccountByTransactionIdResponse,
    CreateBankAccountRequest,
    CreateBankAccountResponse,
    UpdateBankAccountRequest,
    GetCurrencyExchangeByTransactionIdResponse,
    CreateCurrencyExchangeRequest,
    UpdateCurrencyExchangeRequest,
    CreateCurrencyExchangeResponse,
    GetPeerTransferByTransactionIdResponse,
    CreatePeerTransferRequest,
    CreatePeerTransferResponse,
    UpdatePeerTransferRequest
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

export const updateCashFlow = async (request: UpdateCashFlowRequest) => {
    const response = await client.put<ApiResponse>(`wallet/transaction/cashflow/update`, request);
    return response.data;
};

export const getBankAccountByTransactionId = async (transactionId: string) => {
    const response = await client.get<ApiResponse<GetBankAccountByTransactionIdResponse>>(`wallet/transaction/bankaccount/${transactionId}`);
    return response.data; 
};

export const createBankAccount = async (request: CreateBankAccountRequest) => {
    const response = await client.post<ApiResponse<CreateBankAccountResponse>>(`wallet/transaction/bankaccount/create`, request);
    return response.data;
};

export const updateBankAccount = async (request: UpdateBankAccountRequest) => {
    const response = await client.put<ApiResponse>(`wallet/transaction/bankaccount/update`, request);
    return response.data;
};

export const getCurrencyExchangeByTransactionId = async (transactionId: string) => {
    const response = await client.get<ApiResponse<GetCurrencyExchangeByTransactionIdResponse>>(`wallet/transaction/currencyexchange/${transactionId}`);
    return response.data; 
}

export const createCurrencyExchange = async (request: CreateCurrencyExchangeRequest) => {
    const response = await client.post<ApiResponse<CreateCurrencyExchangeResponse>>(`wallet/transaction/currencyexchange/create`, request);
    return response.data;
}

export const updateCurrencyExchange = async (request: UpdateCurrencyExchangeRequest) => {
    const response = await client.put<ApiResponse>(`wallet/transaction/currencyexchange/update`, request);
    return response.data;
}

export const getPeerTransferByTransactionId = async (transactionId: string) => {
    const response = await client.get<ApiResponse<GetPeerTransferByTransactionIdResponse>>(`wallet/transaction/peertransfer/${transactionId}`);
    return response.data; 
}

export const createPeerTransfer = async (request: CreatePeerTransferRequest) => {
    const response = await client.post<ApiResponse<CreatePeerTransferResponse>>(`wallet/transaction/peertransfer/create`, request);
    return response.data;
}

export const updatePeerTransfer = async (request: UpdatePeerTransferRequest) => {
    const response = await client.put<ApiResponse>(`wallet/transaction/peertransfer/update`, request);
    return response.data;
}