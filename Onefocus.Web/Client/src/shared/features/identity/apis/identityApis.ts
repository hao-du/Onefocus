import { AuthenticationRequest, AuthenticationResponse } from './interfaces';
import { ApiResponse } from '../../../hooks';
import client  from '../../../hooks/client/client';

export const refreshToken = async () => {
    const response = await client.get<ApiResponse<AuthenticationResponse>>('identity/refresh');
    return response.data.value.token;
};

export const authenticate = async (request: AuthenticationRequest) => {
    const response = await client.post<ApiResponse<AuthenticationResponse>>('identity/authenticate', request);
    return response.data;
};
