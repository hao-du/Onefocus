import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateBankRequest from "./interfaces/bank/CreateBankRequest";
import CreateBankResponse from "./interfaces/bank/CreateBankResponse";
import GetBanksResponse from "./interfaces/bank/GetBanksResponse";
import GetBankByIdResponse from "./interfaces/bank/GetBankByIdResponse";
import UpdateBankRequest from "./interfaces/bank/UpdateBankRequest";
import GetBanksRequest from "./interfaces/bank/GetBanksRequest";

const bankApi = {
    getBanks: async (request: GetBanksRequest) => {
        const response = await client.post<ApiResponse<GetBanksResponse>>(`wallet/bank/get`, request);
        return response.data;
    },
    getBankById: async (id: string) => {
        const response = await client.get<ApiResponse<GetBankByIdResponse>>(`wallet/bank/${id}`);
        return response.data;
    },
    createBank: async (request: CreateBankRequest) => {
        const response = await client.post<ApiResponse<CreateBankResponse>>(`wallet/bank/create`, request);
        return response.data;
    },
    updateBank: async (request: UpdateBankRequest) => {
        const response = await client.put<ApiResponse>(`wallet/bank/update`, request);
        return response.data;
    },
};

export default bankApi;

