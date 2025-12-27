import AuthenticationResponse from "../../apis/interfaces/AuthenticationResponse";
import ApiResponse from "../../apis/interfaces/ApiResponse";

export default interface AuthContextValue {
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<ApiResponse<AuthenticationResponse>>;
    logout: () => Promise<void>;
}