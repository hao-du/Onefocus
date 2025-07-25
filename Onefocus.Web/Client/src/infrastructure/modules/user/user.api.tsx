import {AxiosInstance} from 'axios';
import {ApiResponse} from '../../hooks';
import {UsersRepsonse} from './user.interfaces';

export const getAllUsers = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<UsersRepsonse>>('membership/user/all');
    return response.data;
};