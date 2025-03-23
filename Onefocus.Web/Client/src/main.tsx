import {StrictMode} from 'react';
import {createRoot} from 'react-dom/client';
import {BrowserRouter, Route, Routes} from "react-router";
import {PrimeReactProvider} from "primereact/api";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";

import App from './pages/App';
import Login from "./pages/Login";

import 'primereact/resources/themes/mira/theme.css';
import './index.scss';
import {AuthProvider} from "./hooks/authentication/useAuth.tsx";
import {ClientProvider} from "./api/client/useClient.tsx";

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
