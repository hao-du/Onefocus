import { AxiosError } from "axios";
import { PropsWithChildren } from "react";
import { ApiResponseBase } from "../../../hooks";
import { useWindows } from "../windows";
import ErrorBoundary from "./ErrorBoundary";

const ErrorHandlerProvider = (props: PropsWithChildren) => {
    const { showToast, showResponseToast } = useWindows();

    return (
        <ErrorBoundary onError={(error: Error) => {
            if (error instanceof AxiosError) {
                const axiosError = error as AxiosError<ApiResponseBase>;
                
                if (axiosError.response) {
                    // If the error has a response, show a toast with the response details
                    showResponseToast(axiosError.response.data, axiosError.message);
                    return;
                }
            }

            showToast({ severity: "error", detail: error.message });
        }}>
            {props.children}
        </ErrorBoundary>
    );
}

export default ErrorHandlerProvider;