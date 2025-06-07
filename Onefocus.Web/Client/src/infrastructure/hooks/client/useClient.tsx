import axios from 'axios';
import {createContext, useContext, useState} from 'react';
import {useNavigate} from 'react-router';
import {useQuery} from '@tanstack/react-query';
import {useAuth} from '../authentication/useAuth';
import {refreshToken} from '../../modules/authentication/authentication.api';
import { ClientContextValue } from "./useClient.interfaces";

const apiUrl = import.meta.env.VITE_API_URL;

const client = axios.create({
    baseURL: apiUrl,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});

const ClientContext = createContext<ClientContextValue>({
    client: client,
    isClientReady: false,
});

const ClientProvider: React.FC<{ children: React.ReactNode }> = ({children}) => {
    const [isClientReady, setIsClientReady] = useState(false);
    const {token, setToken} = useAuth();
    const {client} = useClient();
    const navigate = useNavigate();

    useQuery({
        queryKey: [token],
        queryFn: () => {
            if (token) {
                client.defaults.headers.common["Authorization"] = 'Bearer ' + token;
            } else {
                delete client.defaults.headers.common['Authorization'];
            }

            client.interceptors.response.use(
                response => response,
                async (error) => {
                    const originalRequest = error.config;

                    //_retry is custom property
                    if (error.response.status === 401 && !originalRequest._retry) {
                        originalRequest._retry = true;

                        const response = await refreshToken(client);
                        setToken(response.value.token);

                        //to force it throws exception in order to get response http status for retry.
                        return Promise.reject(error);
                    }
                    if (error.response.status === 406) {
                        navigate('/login');
                    }

                    return {... error.response, ignoredError: true  };
                }
            );

            setIsClientReady(true);
        }
    });

    // Provide the authentication context to the children components
    return (
        <ClientContext.Provider value={{client, isClientReady}}>
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

