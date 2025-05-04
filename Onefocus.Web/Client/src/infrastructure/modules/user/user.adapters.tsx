import {UserResponse, UsersRepsonse} from './user.interfaces';
import {User} from '../../../domain/user';

const toUserEntity = (response: UserResponse): User => {
    return {
        id: response.id,
        userName: response.userName,
        email: response.email,
        firstName: response.firstName,
        lastName: response.lastName,
    };
}

export const toUserEntities = (response: UsersRepsonse): User[] => {
    return response.users.map(user => toUserEntity(user));
}