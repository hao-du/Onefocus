import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import GetAllLocaleOptionsResponse from "./interfaces/settings/GetAllLocaleOptionsResponse";
import GetAllTimeZonesResponse from "./interfaces/settings/GetAllTimeZonesResponse";
import UpsertSettingsRequest from "./interfaces/settings/UpsertSettingsRequest";

const settingsApi = {
    getAllLocaleOptions: async () => {
        const response = await client.get<ApiResponse<GetAllLocaleOptionsResponse>>(`home/settings/option/locales`);
        return response.data;
    },

    getAllTimeZoneOptions: async () => {
        const response = await client.get<ApiResponse<GetAllTimeZonesResponse>>(`home/settings/option/timezones`);
        return response.data;
    },

    upsertSettings: async (request: UpsertSettingsRequest) => {
        const response = await client.post<ApiResponse>(`home/settings/upsert`, request);
        return response.data;
    },
}

export default settingsApi;