import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Route, Routes } from 'react-router';
import { QueryClient } from '@tanstack/react-query';
import Login from './shared/features/identity/pages/Login';
import App from './app/App';
import Main from './shared/app/Main';
import { refreshToken } from './shared/features/identity/apis';

import 'primereact/resources/themes/mira/theme.css';
import './index.scss';

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <Main queryClient={queryClient} unauthorizedCallback={refreshToken}>
            <Routes>
                <Route path="login" element={<Login />} />
                <Route path="*" element={<App />} />
            </Routes>
        </Main>
    </StrictMode>,
);