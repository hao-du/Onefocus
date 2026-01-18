import type { CSSProperties, ReactNode } from "react";

export interface ChildrenProps {
    children?: ReactNode
}

export interface IdentityProps {
    key?: number | string;
    id?: string;
}

export interface NameProps {
    name?: string;
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

export interface FocusProps {
    focus?: boolean;
}

export interface InteractionProps {
    disabled?: boolean;
    isPending?: boolean;
}

export interface ReadOnlyProps {
    readOnly?: boolean;
}