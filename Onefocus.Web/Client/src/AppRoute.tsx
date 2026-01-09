import { Outlet, Route, Routes } from "react-router";
import LoginPage from "./features/login/LoginPage";
import App from "./App";
import BankPage from "./features/wallet/bank/BankPage";
import useAuth from "./shared/hooks/auth/useAuth";
import LoadingPage from "./shared/pages/LoadingPage";

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
            </Route>
            <Route path="/*" element={<App />} />
        </Routes>
    );
}

export default AppRoute;
