import { QueryClient } from '@tanstack/react-query';
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { Route, Routes } from 'react-router';
import App from './app/App';
import { Main } from './shared/app';
import { Login, refreshToken } from './shared/features/identity';
import "./shared/hooks/locale/config";

import 'primereact/resources/themes/tailwind-light/theme.css';
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