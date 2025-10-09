import { AuthenticationResponse } from "../../../features/identity/apis";
import { ApiResponse } from "../../client";

export default interface AuthContextValue {
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<ApiResponse<AuthenticationResponse>>;
}