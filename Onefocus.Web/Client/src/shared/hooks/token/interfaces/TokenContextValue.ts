import React from 'react';

export default interface TokenContextValue {
    token: string | null;
    setToken: React.Dispatch<React.SetStateAction<string | null>>;
}