import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateBankRequest from "./interfaces/CreateBankRequest";
import CreateBankResponse from "./interfaces/CreateBankResponse";
import GetAllBanksResponse from "./interfaces/GetAllBanksResponse";
import GetBankByIdResponse from "./interfaces/GetBankByIdResponse";
import UpdateBankRequest from "./interfaces/UpdateBankRequest";

const bankApi = {
    getAllBanks: async () => {
        const response = await client.get<ApiResponse<GetAllBanksResponse>>(`wallet/bank/all`);
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

