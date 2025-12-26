import type { CSSProperties, MouseEventHandler } from "react";
import type { IdentityProps, InteractionProps, ClassNameProps, IconProps } from "./BaseProps";

export interface BaseButtonProps extends IdentityProps, ClassNameProps, InteractionProps, IconProps {
    onClick?: MouseEventHandler<HTMLElement>;

    styles?: {
        root?: CSSProperties;
        icon?: CSSProperties;
        content?: CSSProperties;
    }
}