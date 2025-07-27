import BaseProps from "./BaseProps";
import {MouseEventHandler} from "react";

type BaseButtonProps = BaseProps & {
    id?:string;
    label?: string;
    icon?: string;
    disabled?: boolean;
    onClick?: MouseEventHandler<HTMLButtonElement>;
    isPending?: boolean;
};

export default BaseButtonProps;