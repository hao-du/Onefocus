import BaseProps from "./BaseProps";

type InputProps = BaseProps & {
    value?: string;
    readOnly?: boolean;
    isPending?: boolean;
    required?: boolean;
    invalid?: boolean;
};

export default InputProps;