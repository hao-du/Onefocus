import {useMutation} from '@tanstack/react-query';
import {useNavigate} from 'react-router';
import {ApiResponse, useAuth, useClient} from '../../infrastructure/hooks';
import {authenticate, AuthenticationRequest, AuthenticationResponse} from '../../infrastructure/modules/authentication';


export const useLogin = () => {
    const {setToken} = useAuth();
    const navigate = useNavigate();
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse<AuthenticationResponse>, unknown, AuthenticationRequest>({
        mutationFn: async (request) => {
            const response = await authenticate(client, request);
            if (response.status === 200) {
                setToken(response.value.token);
                navigate('/wallet');
            } else {
                setToken(null);
            }
            return response;
        }
    });

    return {onLoginAsync: mutateAsync, isPending};
};
