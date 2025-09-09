import BaseProps from "./BaseProps";

type InputProps = BaseProps & {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    required?: boolean;
    invalid?: boolean;
    placeholder?: string;
};

export default InputProps;