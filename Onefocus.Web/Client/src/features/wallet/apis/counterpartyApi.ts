import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateCounterpartyRequest from "./interfaces/counterparty/CreateCounterpartyRequest";
import CreateCounterpartyResponse from "./interfaces/counterparty/CreateCounterpartyResponse";
import GetAllCounterpartiesResponse from "./interfaces/counterparty/GetAllCounterpartiesResponse";
import GetCounterpartyByIdResponse from "./interfaces/counterparty/GetCounterpartyByIdResponse";
import UpdateCounterpartyRequest from "./interfaces/counterparty/UpdateCounterpartyRequest";

const counterpartyApi = {
    getAllCounterparties: async () => {
        const response = await client.get<ApiResponse<GetAllCounterpartiesResponse>>(`wallet/counterparty/all`);
        return response.data;
    },

    getCounterpartyById: async (id: string) => {
        const response = await client.get<ApiResponse<GetCounterpartyByIdResponse>>(`wallet/counterparty/${id}`);
        return response.data;
    },

    createCounterparty: async (request: CreateCounterpartyRequest) => {
        const response = await client.post<ApiResponse<CreateCounterpartyResponse>>(`wallet/counterparty/create`, request);
        return response.data;
    },

    updateCounterparty: async (request: UpdateCounterpartyRequest) => {
        const response = await client.put<ApiResponse>(`wallet/counterparty/update`, request);
        return response.data;
    },
};

export default counterpartyApi;