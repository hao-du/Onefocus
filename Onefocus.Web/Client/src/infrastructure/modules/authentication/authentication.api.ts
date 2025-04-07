import {AxiosInstance} from "axios";
import {AuthenticationRequest, AuthenticationResponse} from "./authentication.interfaces";

export const refreshToken = async (client: AxiosInstance) => {
    const response = await client.get<AuthenticationResponse>('identity/refresh');
    return response.data;
};

export const authenticate = async (client: AxiosInstance, request: AuthenticationRequest) => {
    const response = await client.post<AuthenticationResponse>('identity/authenticate', request);
    return response.data;
};

