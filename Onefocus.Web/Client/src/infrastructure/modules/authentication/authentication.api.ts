import {AxiosInstance} from 'axios';
import {AuthenticationRequest, AuthenticationResponse} from './authentication.interfaces';
import {ApiResponse} from '../../hooks';

export const refreshToken = async (client: AxiosInstance) => {
    const response = await client.get<ApiResponse<AuthenticationResponse>>('identity/refresh');
    return response.data;
};

export const authenticate = async (client: AxiosInstance, request: AuthenticationRequest) => {
    const response = await client.post<ApiResponse<AuthenticationResponse>>('identity/authenticate', request);
    return response.data;
};
