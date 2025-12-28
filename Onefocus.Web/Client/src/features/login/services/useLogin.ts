import { useMutation } from "@tanstack/react-query";
import useAuth from "../../../shared/hooks/auth/useAuth";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import AuthenticationResponse from "../../../shared/apis/interfaces/AuthenticationResponse";
import AuthenticationRequest from "../../../shared/apis/interfaces/AuthenticationRequest";

const useLogin = () => {
    const { login } = useAuth();

    const { mutateAsync, isPending } = useMutation<ApiResponse<AuthenticationResponse>, unknown, AuthenticationRequest>({
        mutationFn: async (request) => {
            const response = await login(request.email, request.password);
            return response;
        }
    });

    return { loginAsync: mutateAsync, isPending };
};

export default useLogin;