import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './global.css';
import './index.scss';
import Main from './shared/app/Main.tsx';
import AppRoute from './AppRoute.tsx';

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <Main>
            <AppRoute />
        </Main>
    </StrictMode >,
);
