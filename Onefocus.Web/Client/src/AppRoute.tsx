import { Outlet, Route, Routes } from "react-router";
import LoginPage from "./features/login/LoginPage";
import App from "./App";
import useAuth from "./shared/hooks/auth/useAuth";
import LoadingPage from "./shared/pages/LoadingPage";
import BankPage from "./features/wallet/bank/BankPage";
import CurrencyPage from "./features/wallet/currency/CurrencyPage";
import CounterpartyPage from "./features/wallet/counterparty/CounterpartyPage";
import TransactionPage from "./features/wallet/transaction/TransactionPage";

const AppRoute = () => {
    const { isAuthReady } = useAuth();

    if (!isAuthReady) {
        return <LoadingPage />
    }

    return (
        <Routes>
            <Route index element={<App />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/wallet" element={<Outlet />}>
                <Route path="bank" element={<BankPage />} />
                <Route path="currency" element={<CurrencyPage />} />
                <Route path="counterparty" element={<CounterpartyPage />} />
                <Route path="transaction" element={<TransactionPage />} />
            </Route>
            <Route path="/*" element={<App />} />
        </Routes>
    );
}

export default AppRoute;
