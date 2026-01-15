import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateCurrencyRequest from "./interfaces/currency/CreateCurrencyRequest";
import CreateCurrencyResponse from "./interfaces/currency/CreateCurrencyResponse";
import GetAllCurrenciesResponse from "./interfaces/currency/GetAllCurrenciesResponse";
import GetCurrencyByIdResponse from "./interfaces/currency/GetCurrencyByIdResponse";
import UpdateCurrencyRequest from "./interfaces/currency/UpdateCurrencyRequest";

const currencyApi = {
    getAllCurrencies: async () => {
        const response = await client.get<ApiResponse<GetAllCurrenciesResponse>>(`wallet/currency/all`);
        return response.data;
    },

    getCurrencyById: async (id: string) => {
        const response = await client.get<ApiResponse<GetCurrencyByIdResponse>>(`wallet/currency/${id}`);
        return response.data;
    },

    createCurrency: async (request: CreateCurrencyRequest) => {
        const response = await client.post<ApiResponse<CreateCurrencyResponse>>(`wallet/currency/create`, request);
        return response.data;
    },

    updateCurrency: async (request: UpdateCurrencyRequest) => {
        const response = await client.put<ApiResponse>(`wallet/currency/update`, request);
        return response.data;
    },
};

export default currencyApi;