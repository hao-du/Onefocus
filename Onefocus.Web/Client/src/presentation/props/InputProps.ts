import {BaseProps} from "./BaseProps";
import {ChangeEventHandler} from "react";

export type InputProps = BaseProps & {
    id?: string;
    value?: string;
    label: string;
    readOnly?: boolean;
    onChange?: ChangeEventHandler<HTMLInputElement>;
    isPending?: boolean;
    required?: boolean;
};