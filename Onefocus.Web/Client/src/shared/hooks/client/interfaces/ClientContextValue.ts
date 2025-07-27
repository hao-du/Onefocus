import { AxiosInstance } from "axios";

export default interface ClientContextValue {
    isClientReady: boolean;
    client: AxiosInstance;
}