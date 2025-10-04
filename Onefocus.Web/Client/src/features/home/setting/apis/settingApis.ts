import { ApiResponse, client } from '../../../../shared/hooks';
import {
    UpsertSettingRequest,
    GetSettingByUserIdResponse,
    GetAllLocaleOptionsResponse,
    GetAllTimeZonesResponse,
} from './interfaces';

export const getSettingByUserId = async () => {
    const response = await client.get<ApiResponse<GetSettingByUserIdResponse>>(`home/setting/get`);
    return response.data;
};

export const getAllLocaleOptions = async () => {
    const response = await client.get<ApiResponse<GetAllLocaleOptionsResponse>>(`home/setting/option/locales`);
    return response.data;
};

export const getAllTimeZoneOptions = async () => {
    const response = await client.get<ApiResponse<GetAllTimeZonesResponse>>(`home/setting/option/timezones`);
    return response.data;
};

export const upsertSetting = async (request: UpsertSettingRequest) => {
    const response = await client.post<ApiResponse>(`home/setting/upsert`, request);
    return response.data;
};