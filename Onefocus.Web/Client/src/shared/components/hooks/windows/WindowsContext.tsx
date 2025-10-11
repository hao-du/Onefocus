import { createContext } from 'react';
import WindowsContextValue from './interfaces/WindowsContextValue';

const WindowsContext = createContext<WindowsContextValue>({
    isMobile: false,
    showToast: () => {
    },
    showResponseToast: () => {
    },
    life: 3000,
    originalUrl: '',
    setOriginalUrl: () => { },
});

export default WindowsContext;