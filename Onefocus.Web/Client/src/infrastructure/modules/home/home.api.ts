import {AxiosInstance} from 'axios';
import {CheckResponse} from './home.interfaces';

export const check = async (client: AxiosInstance) => {
    const response = await client.head<CheckResponse>('home/check');
    return response;
};

