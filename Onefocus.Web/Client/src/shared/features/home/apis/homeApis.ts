import { ApiResponse, client } from '../../../hooks';
import CheckResponse from './interfaces/CheckResponse';

const check = async () => {
    const response = await client.head<ApiResponse<CheckResponse>>('home/check');
    return response;
};

export default check;