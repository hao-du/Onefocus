import BaseProps from "./BaseProps";

type InputProps = BaseProps & {
    id?: string;
    value?: string;
    label?: string;
    readOnly?: boolean;
    isPending?: boolean;
    required?: boolean;
    invalid?: boolean;
    description?: string;
    errorMessage?: string;
};

export default InputProps;