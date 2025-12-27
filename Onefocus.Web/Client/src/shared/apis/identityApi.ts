import client from './client';
import ApiResponse from './interfaces/ApiResponse';
import AuthenticationRequest from './interfaces/AuthenticationRequest';
import AuthenticationResponse from './interfaces/AuthenticationResponse';

const identityApi = {
    refreshToken: async () => {
        const response = await client.get<ApiResponse<AuthenticationResponse>>('identity/refresh');
        return response.data.value;
    },
    authenticate: async (request: AuthenticationRequest) => {
        const response = await client.post<ApiResponse<AuthenticationResponse>>('identity/authenticate', request);
        return response.data;
    },
    logout: async () => {
        const response = await client.post<ApiResponse>('identity/logout');
        return response.data;
    }
}

export default identityApi;