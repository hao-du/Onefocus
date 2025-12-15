import { CSSProperties, ReactNode } from "react";

export interface BaseIdentityProps {
    id?:string;
    key?:string;
};

export interface BaseHtmlProps {
    className?: string;
};

export interface BaseStyleProps {
    style?: CSSProperties;
};

export interface ChildrenProps {
    children?: ReactNode;
}