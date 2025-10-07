import { PropsWithChildren, useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router';
import { useToken } from '../token';
import ClientContext from './ClientContext';
import client from './client';

type ClientProviderProps = PropsWithChildren & {
    unauthorizedCallback?: () => Promise<string>;
};

const ClientProvider = (props: ClientProviderProps) => {
    const [isClientReady, setIsClientReady] = useState(false);
    const { token, setToken } = useToken();
    const navigate = useNavigate();
    const isRefreshing = useRef(false);
    const interceptorId = useRef<number | null>(null);
    const unauthorizedCallbackRef = useRef(props.unauthorizedCallback);

    useEffect(() => {
        unauthorizedCallbackRef.current = props.unauthorizedCallback;
    }, [props.unauthorizedCallback]);

    useEffect(() => {
        // Add interceptor only once
        if (interceptorId.current === null) {
            interceptorId.current = client.interceptors.response.use(
                (response) => response,
                async (error) => {
                    const { response } = error;

                    if (response?.status === 401) {
                        setIsClientReady(false);
                        if (!isRefreshing.current) {
                            isRefreshing.current = true;
                            try {
                                const unauthorizedCallback = unauthorizedCallbackRef.current;
                                if (unauthorizedCallback) {
                                    const newToken = await unauthorizedCallback();
                                    setToken(newToken);
                                    setIsClientReady(true);
                                    return Promise.reject(error);
                                }
                            } finally {
                                isRefreshing.current = false;
                            }

                            return Promise.reject(error);
                        }
                    }

                    if (response?.status === 406) {
                        setIsClientReady(true);
                        navigate('/login');
                    }

                    return Promise.reject(error);
                }
            );

            setIsClientReady(true);
        };
    }, [navigate, setToken]);

    useEffect(() => {
        if (token) {
            client.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        } else {
            delete client.defaults.headers.common['Authorization'];
        }
    }, [token]);

    return (
        <ClientContext.Provider value={{ client, isClientReady }}>
            {props.children}
        </ClientContext.Provider>
    );
};

export default ClientProvider;