import { AxiosError } from "axios";
import { PropsWithChildren, useEffect } from "react";
import { ApiResponseBase } from "../../../hooks";
import { useWindows } from "../windows";
import ErrorBoundary from "./ErrorBoundary";

const ErrorHandlerProvider = (props: PropsWithChildren) => {
    const { showToast, showResponseToast } = useWindows();

    useEffect(() => {
        const handleError = (event: ErrorEvent) => {
            event.preventDefault();
            if (event.error?.isAxiosError && event.error.response) {
                showResponseToast(event.error.response.data, event.error.message);
            } else {
                showToast({ severity: "error", detail: event.error?.message ?? "Unknown error" });
            }
        };

        window.addEventListener("error", handleError);

        return () => {
            window.removeEventListener("error", handleError);
        };
    }, [showToast, showResponseToast]);

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