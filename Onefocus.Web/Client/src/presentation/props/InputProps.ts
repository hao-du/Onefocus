import {BaseProps} from "./BaseProps";

export type InputProps = BaseProps & {
    id?: string;
    value?: string;
    label: string;
    readOnly?: boolean;
    isPending?: boolean;
    required?: boolean;
    description?: string;
};