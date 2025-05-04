import {StrictMode} from 'react';
import {createRoot} from 'react-dom/client';
import {BrowserRouter, Route, Routes} from 'react-router';
import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {PrimeReactProvider} from 'primereact/api';
import {AuthProvider, ClientProvider} from './infrastructure/hooks';
import {Login} from './presentation/pages';
import App from './App';

import 'primereact/resources/themes/saga-orange/theme.css';
import './index.scss';

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <PrimeReactProvider>
                <QueryClientProvider client={queryClient}>
                    <AuthProvider>
                        <ClientProvider>
                            <Routes>
                                <Route path="login" element={<Login/>}/>
                                <Route path="*" element={<App/>}/>
                            </Routes>
                        </ClientProvider>
                    </AuthProvider>
                </QueryClientProvider>
            </PrimeReactProvider>
        </BrowserRouter>
    </StrictMode>,
);