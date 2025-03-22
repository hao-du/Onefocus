import {useClient} from "../client/useClient";
import {ApiResponse} from "../client/models/ApiResponse.ts";
import UsersDto from "./models/UserDto";

const useUserApi = () => {
    const {client} = useClient();

    const getAllUsers = async () => {
        console.log("getAllUsers");
        const response = await client.get<ApiResponse<UsersDto>>('membership/user/all');
        return response.data;
    };

    return {
        getAllUsers
    };
};

export default useUserApi;

