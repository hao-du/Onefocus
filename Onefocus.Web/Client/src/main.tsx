import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './global.css';
import './index.scss';
import App from './App.tsx';
import ThemeProvider from './shared/hooks/theme/ThemeProvider.tsx';
import ClientProvider from './shared/hooks/client/ClientProvider.tsx';
import WindowsProvider from './shared/hooks/windows/WindowsProvider.tsx';
import LocaleProvider from './shared/hooks/locale/LocaleProvider.tsx';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ClientProvider>
            <ThemeProvider>
                <LocaleProvider>
                    <WindowsProvider>
                        <App />
                    </WindowsProvider>
                </LocaleProvider>
            </ThemeProvider>
        </ClientProvider>
    </StrictMode >,
);
