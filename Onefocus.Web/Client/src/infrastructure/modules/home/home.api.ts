import {AxiosInstance} from 'axios';
import {CheckResponse} from './home.interfaces';
import {ApiResponse} from '../../hooks';

export const check = async (client: AxiosInstance) => {
    const response = await client.head<ApiResponse<CheckResponse>>('home/check');
    return response;
};

