/// <reference types="vite/client" />
import axios from "axios";
import tokenManager from "./TokenManager";
import identityApi from "./identityApi";

const API_BASE = import.meta.env.VITE_API_URL;

const client = axios.create({
    baseURL: API_BASE,
    withCredentials: true, // send HttpOnly cookies
});

client.interceptors.request.use((config) => {
    const token = tokenManager.getToken();
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

client.interceptors.response.use(
    (res) => res,
    async (error) => {
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            if (!tokenManager.getRefreshPromise()) {
                const refreshPromise = identityApi.refreshToken()
                    .then((response) => {
                        tokenManager.setToken(response.token, response.expiresAtUtc);
                        return response.token;
                    })
                    .catch((error) => {
                        tokenManager.clear();
                        window.location.reload();
                        throw error;
                    })
                    .finally(() => {
                        tokenManager.setRefreshPromise(null);
                    });

                tokenManager.setRefreshPromise(refreshPromise);
            }

            const newToken = await tokenManager.getRefreshPromise();
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
            return client(originalRequest);
        }

        return Promise.reject(error);
    }
);

export default client;