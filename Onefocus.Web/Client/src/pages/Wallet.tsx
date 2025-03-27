import {Route, Routes} from "react-router";

import Dashboard from "../modules/wallet/Dashboard.tsx";
import Bank from "../modules/wallet/Bank.tsx";
import NotFound from "./NotFound.tsx";
import DefaultLayout from "../components/layouts/DefaultLayout.tsx";

function Wallet() {
    return (
        <DefaultLayout>
            Wallet
            <Routes>
                <Route index element={<Dashboard/>}/>
                <Route path="/bank" element={<Bank/>}/>
                <Route path="/*" element={<NotFound/>}/>
            </Routes>
        </DefaultLayout>
    );
}

export default Wallet;