import BaseProps from "./BaseProps";

type InputProps = BaseProps & {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    required?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    textOnly?: boolean;
};

export default InputProps;