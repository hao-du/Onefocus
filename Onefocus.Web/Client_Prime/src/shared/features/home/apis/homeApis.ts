import { ApiResponse, client } from '../../../hooks';
import { GetSettingsByUserIdResponse } from './interfaces';

export const getSettingsByUserId = async () => {
    const response = await client.get<ApiResponse<GetSettingsByUserIdResponse>>(`home/settings/get`);
    return response.data;
};