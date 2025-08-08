import { PropsWithChildren } from "react";
import { ErrorBoundary, FallbackProps } from "react-error-boundary";
import { useWindows } from "../hooks";

type ErrorHandlerProps = PropsWithChildren & {
};

const ErrorHandler = (props: ErrorHandlerProps) => {
    const {showToast} = useWindows();

    const fallbackRender = (props: FallbackProps) => {
        showToast(props.error.message);
        return null
    };

    return (
        <ErrorBoundary fallbackRender={fallbackRender}>
            {props.children}
        </ErrorBoundary>
    );
}

export default ErrorHandler;