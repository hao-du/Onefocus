/* eslint-disable @typescript-eslint/no-empty-object-type */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { ChildrenProps } from '../../props/BaseProps';
import identityApi from '../../apis/identityApi';
import AuthContext from './AuthContext';
import tokenManager from '../../apis/TokenManager';
import ApiResponse from '../../apis/interfaces/ApiResponse';
import AuthenticationResponse from '../../apis/interfaces/AuthenticationResponse';
import { CHECK_ACCESS_TOKEN_INTERVAL_MILLISECOND } from '../../constants';

interface AuthProviderProps extends ChildrenProps {
}

const AuthProvider = (props: AuthProviderProps) => {
    const [token, setToken] = useState<string | null>(tokenManager.getToken());
    const [isFirstAuthCheck, setIsFirstAuthCheck] = useState(true);
    const [redirectedUrl, setRedirectedUrl] = useState(window.location.href);

    const navigate = useNavigate();

    const login = useCallback(async (email: string, password: string) => {
        try {
            const response = await identityApi.authenticate({ email, password });
            if (response.status === 200) {
                tokenManager.setToken(response.value.token, response.value.expiresAtUtc);
                setToken(response.value.token);

                if (redirectedUrl && !redirectedUrl.includes('/login')) {
                    const relativeUrl = new URL(redirectedUrl).pathname + new URL(redirectedUrl).search;
                    navigate(relativeUrl);
                }
                else navigate('/');

                return response;
            }
            else {
                tokenManager.clear();
                setToken(null);
                return response;
            }
        } catch (error: any) {
            tokenManager.clear();
            setToken(null);
            return error?.response?.data as ApiResponse<AuthenticationResponse>;
        }
    }, [navigate, redirectedUrl]);

    const logout = useCallback(async () => {
        const response = await identityApi.logout();
        if (response.status === 200) {
            tokenManager.clear();
            setToken(null);
        }
    }, []);

    /**
     * Check refresh token for first call.
     * If it is not a first call and token is invalid, return to login page with redirect url.
     */
    useEffect(() => {
        const refreshTokenForFirstAccess = async () => {
            try {
                const response = await identityApi.refreshToken();
                tokenManager.setToken(response.token, response.expiresAtUtc);
                setToken(response.token);
            } finally {
                setIsFirstAuthCheck(false);
            }
        };

        if (isFirstAuthCheck) {
            refreshTokenForFirstAccess();
            return;
        }

        if (!token) {
            const shouldSaveCurrentUrlBeforeRedirectingToLoginPage = !window.location.href.includes('/login') && redirectedUrl != window.location.href;
            if (shouldSaveCurrentUrlBeforeRedirectingToLoginPage) {
                setRedirectedUrl(window.location.href);
            }
            navigate('/login');
        }
    }, [navigate, token, isFirstAuthCheck, redirectedUrl]);

    /**
     * Proactively refresh the access token before it expires.
     */
    useEffect(() => {
        let isRunning = false;
        const refreshIfNeeded = async () => {
            if (isRunning) return;
            isRunning = true;

            try {
                if (tokenManager.isTokenExpiringSoon()) {
                    const response = await identityApi.refreshToken();
                    tokenManager.setToken(response.token, response.expiresAtUtc);
                    setToken(response.token);
                }
            } catch {
                tokenManager.clear();
                setToken(null);
            } finally {
                isRunning = false;
            }
        };

        const intervalId = setInterval(refreshIfNeeded, CHECK_ACCESS_TOKEN_INTERVAL_MILLISECOND);
        return () => clearInterval(intervalId);
    }, [navigate]);

    return (
        <AuthContext.Provider value={{ isAuthReady: !isFirstAuthCheck, login, logout }}>
            {props.children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;