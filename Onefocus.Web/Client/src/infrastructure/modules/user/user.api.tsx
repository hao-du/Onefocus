import {AxiosInstance} from "axios";
import {ApiResponse} from "../../hooks/client/useClient.interfaces";
import {UsersRepsonse} from "./user.interfaces";

export const getAllUsers = async (client: AxiosInstance) => {
    console.log("getAllUsers");
    const response = await client.get<ApiResponse<UsersRepsonse>>('membership/user/all');
    return response.data;
};