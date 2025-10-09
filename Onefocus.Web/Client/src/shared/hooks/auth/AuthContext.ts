import { createContext } from 'react';
import AuthContextValue from './interfaces/AuthContextValue';
import { ApiResponse } from '../client';
import { AuthenticationResponse } from '../../features/identity/apis';

const AuthContext = createContext<AuthContextValue>({
    isAuthenticated : false,
    login: async () => ({} as ApiResponse<AuthenticationResponse>),
});

export default AuthContext;