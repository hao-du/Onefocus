import { Route, Routes } from "react-router";
import LoginPage from "./features/login/LoginPage";
import App from "./App";

const AppRoute = () => {
    return (
        <Routes>
            <Route index element={<App />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/*" element={<App />} />
        </Routes>
    );
}

export default AppRoute;
