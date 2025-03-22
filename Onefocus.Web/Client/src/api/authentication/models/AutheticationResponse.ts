import TokenResponse from "./TokenResponse.ts";

export type AuthenticationResponse = {
    status: number;
    title: string;
    value: TokenResponse;
}