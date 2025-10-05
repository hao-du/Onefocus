import { createContext } from 'react';
import LocaleContextValue from './interfaces/LocaleValueContext';
import { DEFAULT_LANGUAGE, DEFAULT_LOCALE, DEFAULT_TIMEZONE } from '../constants/Locale';

const LocaleContext = createContext<LocaleContextValue>({
    timeZone: DEFAULT_TIMEZONE,
    language: DEFAULT_LANGUAGE,
    locale: DEFAULT_LOCALE,
    formatDateTime: () => '',
    translate: () => ''
});

export default LocaleContext;