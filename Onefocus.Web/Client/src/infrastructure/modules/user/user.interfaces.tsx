export interface UserResponse {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
}

export interface UsersRepsonse {
    users: UserResponse[];
}
