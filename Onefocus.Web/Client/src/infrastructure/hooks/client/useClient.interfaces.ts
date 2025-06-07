import {AxiosInstance} from 'axios';

interface Error {
    code: string;
    description:string;
}

export interface ApiResponseBase {
    status: number;
    title: string;
    type: string;
    errors?: Error[];
}

export interface ApiResponse<T = void>  extends ApiResponseBase {
    value: T;
}

export type ClientContextValue = {
    isClientReady: boolean;
    client: AxiosInstance;
}