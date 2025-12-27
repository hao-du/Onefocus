import client from "./client";
import ApiResponse from "./interfaces/ApiResponse";
import GetSettingsByUserIdResponse from "./interfaces/GetSettingsByUserIdResponse";

const homeApi = {
    getSettingsByUserId: async () => {
        const response = await client.get<ApiResponse<GetSettingsByUserIdResponse>>('home/settings/get');
        return response.data;
    }
}

export default homeApi;