import { ApiResponse, client } from '../../../../shared/hooks';
import {
    UpsertSettingRequest,
    GetSettingByUserIdResponse,
} from './interfaces';

export const getSettingByUserId = async () => {
    const response = await client.get<ApiResponse<GetSettingByUserIdResponse>>(`home/setting/get`);
    return response.data;
};

export const upsertSetting = async (request: UpsertSettingRequest) => {
    const response = await client.post<ApiResponse>(`home/setting/upsert`, request);
    return response.data;
};