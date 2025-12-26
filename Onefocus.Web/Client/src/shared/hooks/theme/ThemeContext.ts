import { createContext } from 'react';
import type ThemeContextValue from './ThemeContextValue';
import { cssClassDefinitions } from './CssClassDefinitions';

const ThemeContext = createContext<ThemeContextValue>({
    cssClasses: cssClassDefinitions
});

export default ThemeContext;