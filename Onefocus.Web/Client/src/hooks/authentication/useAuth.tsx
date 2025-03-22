import * as React from "react";
import {createContext, useContext, useMemo, useState} from "react";
import {AuthContextValue} from "./models/AuthContextValue";

const AuthContext = createContext<AuthContextValue>({
    token: null,
    setToken: () => void (null)
});

const AuthProvider: React.FC<{ children: React.ReactNode }> = ({children}) => {
    const [token, setToken] = useState(localStorage.getItem("token"));

    const contextValue: AuthContextValue = useMemo(
        () => ({
            token,
            setToken,
        }),
        [token]
    );

    // Provide the authentication context to the children components
    return (
        <AuthContext.Provider value={contextValue}>
            {children}
        </AuthContext.Provider>
    );
};

const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within the AuthProvider');
    }
    return context;
};

export {useAuth, AuthProvider};

