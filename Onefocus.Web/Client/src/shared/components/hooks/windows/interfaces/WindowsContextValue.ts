import { Dispatch, SetStateAction } from "react";
import { ApiResponseBase } from "../../../../hooks";
import { ToastMessage } from "../../../indicators";

export default interface WindowsContextValue {
    isMobile: boolean;
    showToast: (message: ToastMessage | ToastMessage[]) => void;
    showResponseToast: (response: ApiResponseBase, message?: string) => void;
    life?: number,
    originalUrl: string,
    setOriginalUrl: Dispatch<SetStateAction<string>>;
}