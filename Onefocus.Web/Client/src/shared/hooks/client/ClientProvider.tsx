/* eslint-disable @typescript-eslint/no-empty-object-type */
import { useMemo } from 'react';
import { AxiosError } from 'axios';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ChildrenProps } from '../../props/BaseProps';

interface ClientProviderProps extends ChildrenProps {
}

const retry = (failureCount: number, error: Error) => {
    const axiosError = error as AxiosError;

    const status = axiosError.response?.status;
    if (status === 500 && failureCount < 3) {
        return true;
    }
    return false;
};

const ClientProvider = (props: ClientProviderProps) => {
    const value = useMemo(() =>
        new QueryClient({
            defaultOptions: {
                queries: {
                    refetchOnWindowFocus: false,
                    throwOnError: false,
                    retry
                },
                mutations: {
                    throwOnError: false,
                    retry
                }
            }
        })
        , []);

    return (
        <QueryClientProvider client={value}>
            {props.children}
        </QueryClientProvider>
    );
};

export default ClientProvider;
