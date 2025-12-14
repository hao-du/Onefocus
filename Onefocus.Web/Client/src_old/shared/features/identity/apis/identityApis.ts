import { ApiResponse } from '../../../hooks';
import client from '../../../hooks/client/client';
import { AuthenticationRequest, AuthenticationResponse } from './interfaces';

export const refreshToken = async () => {
    const response = await client.get<ApiResponse<AuthenticationResponse>>('identity/refresh');
    return response.data.value;
};

export const authenticate = async (request: AuthenticationRequest) => {
    const response = await client.post<ApiResponse<AuthenticationResponse>>('identity/authenticate', request);
    return response.data;
};

export const logout = async () => {
    const response = await client.post<ApiResponse>('identity/logout');
    return response.data;
}; 