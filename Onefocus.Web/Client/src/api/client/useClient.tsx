import * as React from "react";
import {createContext, useContext} from "react";
import {ClientContextValue} from "./models/ClientContextValue";
import {useAuth} from "../../hooks/authentication/useAuth";
import {useNavigate} from "react-router";
import axios from "axios";
import useAuthenticationApi from "../authentication/useAuthenticationApi.ts";
import {useQuery} from "@tanstack/react-query";

const client = axios.create({
    baseURL: 'http://localhost:5001',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});

const ClientContext = createContext<ClientContextValue>({
    client: client,
});

const ClientProvider: React.FC<{ children: React.ReactNode }> = ({children}) => {
    const {token, setToken} = useAuth();
    const {refreshToken} = useAuthenticationApi();
    const navigate = useNavigate();

    useQuery({
        queryKey: [token],
        queryFn: () => {
            if (token) {
                client.defaults.headers.common["Authorization"] = "Bearer " + token;
            } else {
                delete client.defaults.headers.common["Authorization"];
            }

            client.interceptors.response.use(
                response => response,
                async (error) => {
                    const originalRequest = error.config;

                    //_retry is custom property
                    if (error.response.status === 401 && !originalRequest._retry) {
                        originalRequest._retry = true;

                        const response = await refreshToken();
                        setToken(response.value.token);
                    }
                    if (error.response.status === 406) {
                        navigate('/login');
                    }

                    return Promise.reject(error);
                }
            );
        }
    });

    // Provide the authentication context to the children components
    return (
        <ClientContext.Provider value={{client}}>
            {children}
        </ClientContext.Provider>
    );
};

const useClient = () => {
    const context = useContext(ClientContext);
    if (!context) {
        throw new Error('useClient must be used within the ClientProvider');
    }
    return context;
};

export {useClient, ClientProvider};

