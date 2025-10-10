import { ApiResponse, client } from '../../../../shared/hooks';
import { CreateUserRequest, CreateUserResponse, GetAllUsersQueryResponse, GetUserByIdQueryResponse, UpdatePasswordRequest, UpdateUserRequest } from './interfaces';

export const getAllUsers = async () => {
    const response = await client.get<ApiResponse<GetAllUsersQueryResponse>>(`membership/user/all`);
    return response.data;
};

export const getUserById = async (id: string) => {
    const response = await client.get<ApiResponse<GetUserByIdQueryResponse>>(`membership/user/${id}`);
    return response.data;
};

export const createUser = async (request: CreateUserRequest) => {
    const response = await client.post<ApiResponse<CreateUserResponse>>(`membership/user/create`, request);
    return response.data;
};

export const updateUser = async (request: UpdateUserRequest) => {
    const response = await client.put<ApiResponse>(`membership/user/update`, request);
    return response.data;
};

export const syncUsers = async () => {
    const response = await client.post<ApiResponse>(`membership/user/sync`);
    return response.data;
}

export const updatePassword = async (request: UpdatePasswordRequest) => {
    const response = await client.patch<ApiResponse>(`membership/user/password/update`, request);
    return response.data;
}