import ApiResponseBase from "../../../apis/interfaces/ApiResponseBase";
import ToastMessage from "./ToastMessage";

export default interface WindowsContextValue {
    isMobile: boolean;
    showToast: (message: ToastMessage | ToastMessage[]) => void;
    showResponseToast: (response: ApiResponseBase, message?: string) => void;
}