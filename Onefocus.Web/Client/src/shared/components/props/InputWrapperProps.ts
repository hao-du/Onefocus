import { PropsWithChildren } from "react";

type InputWrapperProps = PropsWithChildren & {
    label?: string;
    htmlFor?: string;
    description?: string;
    errorMessage?: string;
    size?: 'normal' | 'small';
};

export default InputWrapperProps;