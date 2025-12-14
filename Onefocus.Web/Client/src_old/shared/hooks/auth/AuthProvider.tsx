/* eslint-disable @typescript-eslint/no-explicit-any */
import { PropsWithChildren, useCallback, useEffect, useState } from 'react';
import { useNavigate } from 'react-router';
import { authenticate, refreshToken, logout as identityLogout } from '../../features/identity';
import AuthContext from './AuthContext';
import { tokenManager } from '../client/TokenManager';
import { ApiResponse } from '../client';
import { AuthenticationResponse } from '../../features/identity/apis';
import { useWindows } from '../../components/hooks';

type AuthProviderProps = PropsWithChildren & {
};

const AuthProvider = (props: AuthProviderProps) => {
    const [token, setToken] = useState<string | null>(tokenManager.getToken());
    const [isFirstAuthCheck, setIsFirstAuthCheck] = useState(true);
    const navigate = useNavigate();
    const { originalUrl, setOriginalUrl } = useWindows();

    const login = useCallback(async (email: string, password: string) => {
        try {
            const response = await authenticate({ email, password });
            if (response.status === 200) {
                tokenManager.setToken(response.value.token, response.value.expiresAtUtc);
                setToken(response.value.token);

                if (originalUrl && !originalUrl.includes('/login')) {
                    const relativeUrl = new URL(originalUrl).pathname + new URL(originalUrl).search;
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
    }, [navigate, originalUrl]);

    const logout = useCallback(async () => {
        const response = await identityLogout();
        if (response.status === 200) {
            tokenManager.clear();
            setToken(null);
        }
    }, []);

    useEffect(() => {
        if (isFirstAuthCheck) {
            refreshToken()
                .then((response) => {
                    tokenManager.setToken(response.token, response.expiresAtUtc);
                    setToken(response.token);
                })
                .finally(() => {
                    setIsFirstAuthCheck(false)
                });
        }
        else if (!token) {
            if(!window.location.href.includes('/login') && originalUrl != window.location.href) {
                setOriginalUrl(window.location.href);
            }
            navigate('/login');
        }
    }, [navigate, token, isFirstAuthCheck, setOriginalUrl, originalUrl]);

    // proactive refresh
    useEffect(() => {
        const interval = setInterval(async () => {
            if (tokenManager.isTokenExpiringSoon()) {
                try {
                    const response = await refreshToken();
                    tokenManager.setToken(response.token, response.expiresAtUtc);
                    setToken(response.token);
                } catch {
                    tokenManager.clear();
                    setToken(null);
                }
            }
        }, 60_000); // every 1 minute

        return () => clearInterval(interval);
    }, [navigate]);

    return (
        <AuthContext.Provider value={{ isAuthenticated: !!token, login, logout }}>
            {props.children}
        </AuthContext.Provider>
    );
};

export default AuthProvider;