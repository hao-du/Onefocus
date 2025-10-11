import BaseProps from "./BaseProps";
import { MouseEventHandler } from "react";

type BaseButtonProps = BaseProps & {
    id?:string;
    label?: string;
    icon?: string;
    disabled?: boolean;
    onClick?: MouseEventHandler<HTMLButtonElement>;
    isPending?: boolean;
    text?: boolean;
    rounded?: boolean;
    severity?: 'secondary' | 'success' | 'info' | 'warning' | 'danger' | 'help' | 'contrast';
};

export default BaseButtonProps;