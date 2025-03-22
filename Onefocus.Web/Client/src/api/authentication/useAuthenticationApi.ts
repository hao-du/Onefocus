import {useClient} from "../client/useClient";
import {AuthenticationResponse} from "./models/AutheticationResponse";
import {AuthenticationRequest} from "./models/AutheticationRequest";

const useAuthenticationApi = () => {
    const {client} = useClient();

    const refreshToken = async () => {
        console.log("Refresh token");
        const response = await client.get<AuthenticationResponse>('identity/refresh');
        return response.data;
    };

    const authenticate = async (request: AuthenticationRequest) => {
        const response = await client.post<AuthenticationResponse>('identity/authenticate', request);
        return response.data;
    };

    return {
        refreshToken,
        authenticate
    };
};

export default useAuthenticationApi;

