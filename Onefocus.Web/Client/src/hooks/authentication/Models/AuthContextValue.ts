import * as React from "react";

export interface AuthContextValue {
    token: string | null;
    setToken: React.Dispatch<React.SetStateAction<string | null>>;
}