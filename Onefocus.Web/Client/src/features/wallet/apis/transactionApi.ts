import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateBankAccountRequest from "./interfaces/transaction/bank-account/CreateBankAccountRequest";
import CreateBankAccountResponse from "./interfaces/transaction/bank-account/CreateBankAccountResponse";
import GetBankAccountByTransactionIdResponse from "./interfaces/transaction/bank-account/GetBankAccountByTransactionIdResponse";
import UpdateBankAccountRequest from "./interfaces/transaction/bank-account/UpdateBankAccountRequest";
import CreateCashFlowRequest from "./interfaces/transaction/cashflow/CreateCashFlowRequest";
import CreateCashFlowResponse from "./interfaces/transaction/cashflow/CreateCashFlowResponse";
import GetCashFlowByTransactionIdResponse from "./interfaces/transaction/cashflow/GetCashFlowByTransactionIdResponse";
import UpdateCashFlowRequest from "./interfaces/transaction/cashflow/UpdateCashFlowRequest";
import CreateCurrencyExchangeRequest from "./interfaces/transaction/currency-exchange/CreateCurrencyExchangeRequest";
import CreateCurrencyExchangeResponse from "./interfaces/transaction/currency-exchange/CreateCurrencyExchangeResponse";
import GetCurrencyExchangeByTransactionIdResponse from "./interfaces/transaction/currency-exchange/GetCurrencyExchangeByTransactionIdResponse";
import UpdateCurrencyExchangeRequest from "./interfaces/transaction/currency-exchange/UpdateCurrencyExchangeRequest";
import GetAllTransactionsResponse from "./interfaces/transaction/GetAllTransactionsResponse";
import CreatePeerTransferRequest from "./interfaces/transaction/peer-transfer/CreatePeerTransferRequest";
import CreatePeerTransferResponse from "./interfaces/transaction/peer-transfer/CreatePeerTransferResponse";
import GetPeerTransferByTransactionIdResponse from "./interfaces/transaction/peer-transfer/GetPeerTransferByTransactionIdResponse";
import UpdatePeerTransferRequest from "./interfaces/transaction/peer-transfer/UpdatePeerTransferRequest";
import SearchTransactionsRequest from "./interfaces/transaction/search/SearchTransactionsRequest";
import SearchTransactionsResponse from "./interfaces/transaction/search/SearchTransactionsResponse";

const transactionApi = {
    getAllTransactions: async () => {
        const response = await client.get<ApiResponse<GetAllTransactionsResponse>>(`wallet/transaction/all`);
        return response.data;
    },

    searchTransactions: async (request: SearchTransactionsRequest) => {
        const response = await client.post<ApiResponse<SearchTransactionsResponse>>(`wallet/transaction/search`, request);
        return response.data;
    },

    getCashFlowByTransactionId: async (transactionId: string) => {
        const response = await client.get<ApiResponse<GetCashFlowByTransactionIdResponse>>(`wallet/transaction/cashflow/${transactionId}`);
        return response.data;
    },

    createCashFlow: async (request: CreateCashFlowRequest) => {
        const response = await client.post<ApiResponse<CreateCashFlowResponse>>(`wallet/transaction/cashflow/create`, request);
        return response.data;
    },

    updateCashFlow: async (request: UpdateCashFlowRequest) => {
        const response = await client.put<ApiResponse>(`wallet/transaction/cashflow/update`, request);
        return response.data;
    },

    getBankAccountByTransactionId: async (transactionId: string) => {
        const response = await client.get<ApiResponse<GetBankAccountByTransactionIdResponse>>(`wallet/transaction/bankaccount/${transactionId}`);
        return response.data;
    },

    createBankAccount: async (request: CreateBankAccountRequest) => {
        const response = await client.post<ApiResponse<CreateBankAccountResponse>>(`wallet/transaction/bankaccount/create`, request);
        return response.data;
    },

    updateBankAccount: async (request: UpdateBankAccountRequest) => {
        const response = await client.put<ApiResponse>(`wallet/transaction/bankaccount/update`, request);
        return response.data;
    },

    getCurrencyExchangeByTransactionId: async (transactionId: string) => {
        const response = await client.get<ApiResponse<GetCurrencyExchangeByTransactionIdResponse>>(`wallet/transaction/currencyexchange/${transactionId}`);
        return response.data;
    },

    createCurrencyExchange: async (request: CreateCurrencyExchangeRequest) => {
        const response = await client.post<ApiResponse<CreateCurrencyExchangeResponse>>(`wallet/transaction/currencyexchange/create`, request);
        return response.data;
    },

    updateCurrencyExchange: async (request: UpdateCurrencyExchangeRequest) => {
        const response = await client.put<ApiResponse>(`wallet/transaction/currencyexchange/update`, request);
        return response.data;
    },

    getPeerTransferByTransactionId: async (transactionId: string) => {
        const response = await client.get<ApiResponse<GetPeerTransferByTransactionIdResponse>>(`wallet/transaction/peertransfer/${transactionId}`);
        return response.data;
    },

    createPeerTransfer: async (request: CreatePeerTransferRequest) => {
        const response = await client.post<ApiResponse<CreatePeerTransferResponse>>(`wallet/transaction/peertransfer/create`, request);
        return response.data;
    },

    updatePeerTransfer: async (request: UpdatePeerTransferRequest) => {
        const response = await client.put<ApiResponse>(`wallet/transaction/peertransfer/update`, request);
        return response.data;
    }
};

export default transactionApi;