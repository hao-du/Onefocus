import { AuthenticationRequest, AuthenticationResponse } from "./interfaces";
import { refreshToken, authenticate } from "./identityApis";

export {
    refreshToken,
    authenticate
};
export type {
    AuthenticationRequest,
    AuthenticationResponse
};