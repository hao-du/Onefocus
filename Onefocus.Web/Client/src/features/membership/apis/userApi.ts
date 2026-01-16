import client from "../../../shared/apis/client";
import ApiResponse from "../../../shared/apis/interfaces/ApiResponse";
import CreateUserRequest from "./interfaces/CreateUserRequest";
import CreateUserResponse from "./interfaces/CreateUserResponse";
import GetAllUsersQueryResponse from "./interfaces/GetAllUsersQueryResponse";
import GetUserByIdQueryResponse from "./interfaces/GetUserByIdQueryResponse";
import UpdatePasswordRequest from "./interfaces/UpdatePasswordRequest";
import UpdateUserRequest from "./interfaces/UpdateUserRequest";

const userApi = {
    getAllUsers: async () => {
        const response = await client.get<ApiResponse<GetAllUsersQueryResponse>>(`membership/user/all`);
        return response.data;
    },

    getUserById: async (id: string) => {
        const response = await client.get<ApiResponse<GetUserByIdQueryResponse>>(`membership/user/${id}`);
        return response.data;
    },

    createUser: async (request: CreateUserRequest) => {
        const response = await client.post<ApiResponse<CreateUserResponse>>(`membership/user/create`, request);
        return response.data;
    },

    updateUser: async (request: UpdateUserRequest) => {
        const response = await client.put<ApiResponse>(`membership/user/update`, request);
        return response.data;
    },

    syncUsers: async () => {
        const response = await client.post<ApiResponse>(`membership/user/sync`);
        return response.data;
    },

    updatePassword: async (request: UpdatePasswordRequest) => {
        const response = await client.patch<ApiResponse>(`membership/user/password/update`, request);
        return response.data;
    },
};

export default userApi;