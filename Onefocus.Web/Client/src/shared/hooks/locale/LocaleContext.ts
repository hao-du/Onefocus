import { createContext, useContext } from 'react';
import { DEFAULT_LANGUAGE, DEFAULT_LOCALE, DEFAULT_TIMEZONE } from './constants';

export interface LocaleContextValue {
    language: string;
    setLanguage: React.Dispatch<React.SetStateAction<string>>;
    locale: string;
    setLocale: React.Dispatch<React.SetStateAction<string>>;
    timeZone: string;
    setTimeZone: React.Dispatch<React.SetStateAction<string>>;
    formatDateTime: (date: Date | string | number, showTime: boolean) => string;
    translate: (input?: string) => string;
}

export const LocaleContext = createContext<LocaleContextValue>({
    timeZone: DEFAULT_TIMEZONE,
    setTimeZone: () => { },
    language: DEFAULT_LANGUAGE,
    setLanguage: () => { },
    locale: DEFAULT_LOCALE,
    setLocale: () => { },
    formatDateTime: () => '',
    translate: () => '',
});

export const useLocale = () => {
    const context = useContext(LocaleContext);
    if (!context) {
        throw new Error('useLocale must be used within the LocaleProvider');
    }
    return context;
};

