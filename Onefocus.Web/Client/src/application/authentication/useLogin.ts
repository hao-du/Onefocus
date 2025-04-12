import {useMutation} from "@tanstack/react-query";
import {useNavigate} from "react-router";
import { useAuth } from "../../infrastructure/hooks/authentication/useAuth";
import {AuthenticationRequest} from "../../infrastructure/modules/authentication/authentication.interfaces";
import {authenticate} from "../../infrastructure/modules/authentication/authentication.api";
import {useClient} from "../../infrastructure/hooks/client/useClient";

const useLogin = () => {
    const {setToken} = useAuth();
    const navigate = useNavigate();
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<void, unknown, AuthenticationRequest>({
        mutationFn: async (request) => {
            const response = await authenticate(client, request);
            if (response.status === 200) {
                setToken(response.value.token);
                navigate('/wallet');
            } else {
                setToken(null);
            }
        }
    });

    return {mutateAsync, isPending};
};

export default useLogin;
