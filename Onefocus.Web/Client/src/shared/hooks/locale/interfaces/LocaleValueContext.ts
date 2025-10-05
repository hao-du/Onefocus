export default interface LocaleContextValue {
    language: string;
    locale: string;
    timeZone: string;
    formatDateTime: (date: Date | string | number, showTime: boolean) => string;
    translate: (input?: string) => string;
}