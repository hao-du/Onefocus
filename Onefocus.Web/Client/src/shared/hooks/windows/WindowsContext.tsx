import { createContext } from 'react';
import WindowsContextValue from './interfaces/WindowsContextValue';

const WindowsContext = createContext<WindowsContextValue | null>(null);

export default WindowsContext;