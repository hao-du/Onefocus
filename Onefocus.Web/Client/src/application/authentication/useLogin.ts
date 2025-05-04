import {useMutation} from '@tanstack/react-query';
import {useNavigate} from 'react-router';
import {useAuth, useClient} from '../../infrastructure/hooks';
import {authenticate, AuthenticationRequest} from '../../infrastructure/modules/authentication';


export const useLogin = () => {
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
