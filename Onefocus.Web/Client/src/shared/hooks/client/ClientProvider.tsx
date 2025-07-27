import { PropsWithChildren, useState } from 'react';
import { useNavigate } from 'react-router';
import { useQuery } from '@tanstack/react-query';
import ClientContext from './ClientContext';
import client from './client';
import { useToken } from '../token';

type ClientProviderProps = PropsWithChildren & {
    unauthorizedCallback?: () => Promise<string>;
};

const ClientProvider = (props: ClientProviderProps) => {
    const [isClientReady, setIsClientReady] = useState(false);
    const { token, setToken } = useToken();
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

                        if (props.unauthorizedCallback) {
                            const response = await props.unauthorizedCallback();
                            setToken(response);
                        }

                        //to force it throws exception in order to get response http status for retry.
                        return Promise.reject(error);
                    }
                    if (error.response.status === 406) {
                        navigate('/login');
                    }

                    return { ...error.response, ignoredError: true };
                }
            );

            setIsClientReady(true);
        }
    });

    // Provide the authentication context to the children components
    return (
        <ClientContext.Provider value={{ client, isClientReady }}>
            {props.children}
        </ClientContext.Provider>
    );
};

export default ClientProvider;