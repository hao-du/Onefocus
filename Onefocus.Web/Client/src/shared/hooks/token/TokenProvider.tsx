import { useMemo, useState } from 'react';
import TokenContext from './TokenContext';
import TokenContextValue from './interfaces/TokenContextValue';

const TokenProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [token, setToken] = useState<string | null>(null);
    
    const contextValue: TokenContextValue = useMemo(
        () => ({
            token,
            setToken,
        }),
        [token]
    );

    // Provide the authentication context to the children components
    return (
        <TokenContext.Provider value={contextValue}>
            {children}
        </TokenContext.Provider>
    );
};

export default TokenProvider;