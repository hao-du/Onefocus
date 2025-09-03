import { PropsWithChildren } from "react";

type InputWrapperProps = PropsWithChildren & {
    label?: string;
    htmlFor?: string;
    description?: string;
    errorMessage?: string;
};

export default InputWrapperProps;