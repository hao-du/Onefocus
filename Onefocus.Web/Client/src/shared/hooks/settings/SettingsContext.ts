import { createContext } from 'react';
import SettingsContextValue from './SettingsContextValue';

const SettingsContext = createContext<SettingsContextValue | null>(null);

export default SettingsContext;