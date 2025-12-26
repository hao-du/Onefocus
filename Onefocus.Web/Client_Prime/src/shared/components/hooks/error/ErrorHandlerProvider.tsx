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

        const handleUnhandledRejection = (event: PromiseRejectionEvent) => {
            event.preventDefault();
            if(event.reason.status == 406) return;
            if (event.reason instanceof AxiosError && event.reason.response) {
                showResponseToast(event.reason.response.data, event.reason.message);
            } else {
                showToast({ severity: "error", detail: event.reason instanceof Error ? event.reason.message : "Unknown error" });
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
                showToast({ severity: "error", detail: error.message });
            } else {
                showToast({ severity: "error", detail: "Unknown error" });
            }
        }}>
            {props.children}
        </ErrorBoundary>
    );
}

export default ErrorHandlerProvider;