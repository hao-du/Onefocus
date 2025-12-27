
export default interface LocaleContextValue {
    language: string;
    setLanguage: React.Dispatch<React.SetStateAction<string>>;
    locale: string;
    setLocale: React.Dispatch<React.SetStateAction<string>>;
    timeZone: string;
    setTimeZone: React.Dispatch<React.SetStateAction<string>>;
    formatDateTime: (date: Date | string | number, showTime: boolean) => string;
    translate: (input?: string) => string;
}