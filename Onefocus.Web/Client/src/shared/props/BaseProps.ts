import type { CSSProperties, ReactNode } from "react";

export interface ChildrenProps {
    children: ReactNode
}

export interface IdentityProps {
    key?: string;
    id?: string;
}

export interface LabelProps {
    label?: string;
}

export interface ClassNameProps {
    className?: string;
}

export interface StyleProps {
    style?: CSSProperties;
}

export interface IconProps {
    icon?: ReactNode;
}

export interface InteractionProps {
    disabled?: boolean;
    isPending?: boolean;
}