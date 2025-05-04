import {AxiosInstance} from 'axios';

export interface ApiResponse<T> {
    status: number;
    title: string;
    type: string;
    value: T;
}

export type ClientContextValue = {
    isClientReady: boolean;
    client: AxiosInstance;
}