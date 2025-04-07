export interface TokenResponse {
    token: string;
}

export interface AuthenticationRequest {
    email: string;
    password: string;
}

export interface AuthenticationResponse {
    status: number;
    title: string;
    value: TokenResponse;
}
