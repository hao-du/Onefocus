import UserResponse from "./UserResponse";

export default interface GetAllUsersQueryResponse {
    users: UserResponse[];
}