import {useMutation} from '@tanstack/react-query';
import {useNavigate} from 'react-router';
import { ApiResponse, useToken } from '../../../hooks';
import { AuthenticationRequest, AuthenticationResponse } from '../apis/interfaces';
import { authenticate } from '../apis';

const useLogin = () => {
    const {setToken} = useToken();
    const navigate = useNavigate();

    const {mutateAsync, isPending} = useMutation<ApiResponse<AuthenticationResponse>, unknown, AuthenticationRequest>({
        mutationFn: async (request) => {
            const response = await authenticate(request);
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

export default useLogin;