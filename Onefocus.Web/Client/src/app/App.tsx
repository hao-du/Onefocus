import { Route, Routes, useNavigate } from 'react-router';
import { WalletRoutes } from '../features/wallet';
import { Loading, NotFound } from '../shared/app';
import { AppLayout } from '../shared/components/layouts';
import { SideMenuItem } from '../shared/components/navigations';
import { useCheck } from '../shared/features/home';
import { Home } from './pages';
import { useMemo } from 'react';

const App = () => {
    const navigate = useNavigate();
    const { isCheckDone } = useCheck();

    const items = useMemo<SideMenuItem[]>(() => [
        {
            key: 0,
            label: 'Home',
            icon: 'pi pi-home',
            command: () => {
                navigate("/");
            }
        },
        {
            key: 1,
            label: 'Wallet',
            icon: 'pi pi-wallet',
            items: [
                {
                    label: 'Dashboard',
                    icon: 'pi pi-gauge',
                    command: () => {
                        navigate("/wallet");
                    }
                },
                {
                    label: 'Your traces',
                    icon: 'pi pi-calculator',
                    command: () => {
                        navigate("/wallet/transaction");
                    }
                },
                {
                    label: 'Currencies',
                    icon: 'pi pi-dollar',
                    command: () => {
                        navigate("/wallet/currency");
                    }
                },
                {
                    label: 'Banks',
                    icon: 'pi pi-credit-card',
                    command: () => {
                        navigate("/wallet/bank");
                    }
                }
                ,
                {
                    label: 'Counterparties',
                    icon: 'pi pi-users',
                    command: () => {
                        navigate("/wallet/counterparty");
                    }
                }
            ]
        },
        {
            key: 2,
            label: 'Admin',
            icon: 'pi pi-user',
            items: [
                {
                    label: 'Users',
                    icon: 'pi pi-users',
                    command: () => {
                        navigate("/admin/users");
                    }
                },
                {
                    label: 'Settings',
                    icon: 'pi pi-cog',
                    command: () => {
                        navigate("/admin/settings");
                    }
                }
            ]
        }
    ], [navigate]);

    return (
        !isCheckDone ? <Loading /> : (
            <AppLayout items={items}>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="wallet/*" element={<WalletRoutes />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </AppLayout>
        )
    );
}

export default App;
