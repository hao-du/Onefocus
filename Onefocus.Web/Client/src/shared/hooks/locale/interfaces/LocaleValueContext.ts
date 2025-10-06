import { ApiResponseBase } from "../../client";

export default interface LocaleContextValue {
    language: string;
    locale: string;
    timeZone: string;
    formatDateTime: (date: Date | string | number, showTime: boolean) => string;
    translate: (input?: string) => string;
    showTranslatedToast: (response: ApiResponseBase, message?: string) => void;
}