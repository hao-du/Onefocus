import { createContext } from 'react';
import LocaleContextValue from './interfaces/LocaleValueContext';
import { DEFAULT_LANGUAGE, DEFAULT_LOCALE, DEFAULT_TIMEZONE } from '../constants/Locale';

const LocaleContext = createContext<LocaleContextValue>({
    timeZone: DEFAULT_TIMEZONE,
    setTimeZone: () => { },
    language: DEFAULT_LANGUAGE,
    setLanguage: () => { },
    locale: DEFAULT_LOCALE,
    setLocale: () => { },
    formatDateTime: () => '',
    translate: () => '',
});

export default LocaleContext;