import { createContext } from 'react';
import type ThemeContextValue from './ThemeContextValue';

const ThemeContext = createContext<ThemeContextValue | null>(null);

export default ThemeContext;