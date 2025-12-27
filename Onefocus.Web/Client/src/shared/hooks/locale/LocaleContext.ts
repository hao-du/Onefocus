import { createContext } from 'react';
import LocaleContextValue from './LocaleValueContext';

const LocaleContext = createContext<LocaleContextValue | null>(null);

export default LocaleContext;