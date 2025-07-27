import {createContext} from 'react';
import TokenContextValue from './interfaces/TokenContextValue';

const TokenContext = createContext<TokenContextValue>({
    token: null,
    setToken: () => {},
});

export default TokenContext;