import { useEffect } from "react";
import { AxiosError } from "axios";
import ErrorBoundary from "./ErrorBoundary";
import { ChildrenProps } from "../../props/BaseProps";
import useWindows from "../windows/useWindows";
import ApiResponseBase from "../../apis/interfaces/ApiResponseBase";

const ErrorHandlerProvider = (props: ChildrenProps) => {
    const { showToast, showResponseToast } = useWindows();

    useEffect(() => {
        const handleError = (event: ErrorEvent) => {
            event.preventDefault();
            if (event.error?.isAxiosError && event.error.response) {
                showResponseToast(event.error.response.data, event.error.message);
            } else {
                showToast({ severity: "error", description: event.error?.message ?? "Unknown error" });
            }
        };

        const handleUnhandledRejection = (event: PromiseRejectionEvent) => {
            event.preventDefault();
            if (event.reason.status == 406) return;
            if (event.reason instanceof AxiosError && event.reason.response) {
                showResponseToast(event.reason.response.data, event.reason.message);
            } else {
                showToast({ severity: "error", description: event.reason instanceof Error ? event.reason.message : "Unknown error" });
            }
        }

        window.addEventListener("error", handleError);
        window.addEventListener("unhandledrejection", handleUnhandledRejection);

        return () => {
            window.removeEventListener("error", handleError);
            window.removeEventListener("unhandledrejection", handleUnhandledRejection);
        };
    }, [showToast, showResponseToast]);

    return (
        <ErrorBoundary onError={(error: unknown) => {
            const err = error as AxiosError<ApiResponseBase>;
            if ((err as AxiosError)?.isAxiosError && err.response) {
                showResponseToast(err.response.data, err.message);
            } else if (error instanceof Error) {
                showToast({ severity: "error", description: error.message });
            } else {
                showToast({ severity: "error", description: "Unknown error" });
            }
        }}>
            {props.children}
        </ErrorBoundary>
    );
}

export default ErrorHandlerProvider;