import { ApiResponse, client } from '../../../../shared/hooks';
import {
    UpsertSettingsRequest,
    GetAllLocaleOptionsResponse,
    GetAllTimeZonesResponse,
} from './interfaces';

export const getAllLocaleOptions = async () => {
    const response = await client.get<ApiResponse<GetAllLocaleOptionsResponse>>(`home/settings/option/locales`);
    return response.data;
};

export const getAllTimeZoneOptions = async () => {
    const response = await client.get<ApiResponse<GetAllTimeZonesResponse>>(`home/settings/option/timezones`);
    return response.data;
};

export const upsertSettings = async (request: UpsertSettingsRequest) => {
    const response = await client.post<ApiResponse>(`home/settings/upsert`, request);
    return response.data;
};