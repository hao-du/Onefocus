export default interface UserDto {
    id: string;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
}

export default interface UsersDto {
    users: UserDto[];
}