import { Navigate, Outlet, Route, Routes } from "react-router";
import LoginPage from "./features/login/LoginPage";
import useAuth from "./shared/hooks/auth/useAuth";
import LoadingPage from "./shared/pages/LoadingPage";
import BankPage from "./features/wallet/bank/BankPage";
import CurrencyPage from "./features/wallet/currency/CurrencyPage";
import CounterpartyPage from "./features/wallet/counterparty/CounterpartyPage";
import TransactionPage from "./features/wallet/transaction/TransactionPage";
import UserPage from "./features/membership/user/UserPage";
import SettingsPage from "./features/home/settings/SettingsPage";
import DashboardPage from "./features/wallet/dashboard/DashboardPage";
import HomePage from "./features/home/index/HomePage";
import NotFoundDetail from "./features/misc/not-found/NotFoundDetail";

const AppRoute = () => {
    const { isAuthReady } = useAuth();

    if (!isAuthReady) {
        return <LoadingPage />
    }

    return (
        <Routes>
            <Route index element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/wallet" element={<Outlet />}>
                <Route index element={<Navigate to="/wallet/dashboard" replace />} />
                <Route path="bank" element={<BankPage />} />
                <Route path="currency" element={<CurrencyPage />} />
                <Route path="counterparty" element={<CounterpartyPage />} />
                <Route path="transactions" element={<TransactionPage />} />
                <Route path="dashboard" element={<DashboardPage />} />
            </Route>
            <Route path="/membership" element={<Outlet />}>
                <Route index element={<Navigate to="/membership/user" replace />} />
                <Route path="user" element={<UserPage />} />
            </Route>
            <Route path="/home" element={<Outlet />}>
                <Route index element={<Navigate to="/home/settings" replace />} />
                <Route path="settings" element={<SettingsPage />} />
            </Route>
            <Route path="/*" element={<NotFoundDetail />} />
        </Routes>
    );
}

export default AppRoute;
