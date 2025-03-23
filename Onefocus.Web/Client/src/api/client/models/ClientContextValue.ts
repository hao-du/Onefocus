import {AxiosInstance} from "axios";

export type ClientContextValue = {
    isClientReady: boolean;
    client: AxiosInstance;
}