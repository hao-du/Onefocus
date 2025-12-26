import { ApiResponse, useAuth, useMutation } from '../../../hooks';
import { AuthenticationRequest, AuthenticationResponse } from '../apis/interfaces';

const useLogin = () => {
    const {login} = useAuth();

    const {mutateAsync, isPending} = useMutation<ApiResponse<AuthenticationResponse>, unknown, AuthenticationRequest>({
        mutationFn: async (request) => {
            const response = await login(request.email, request.password);
            return response;
        }
    });

    return {onLoginAsync: mutateAsync, isPending};
};

export default useLogin;