import { createContext } from 'react';
import AuthContextValue from './AuthContextValue';

const AuthContext = createContext<AuthContextValue | null>(null);

export default AuthContext;