import {Route, Routes} from "react-router";

import Dashboard from "../modules/wallet/Dashboard";
import Bank from "../modules/wallet/Bank";
import NotFound from "./NotFound";
import AppLayout from "../layouts/AppLayout";

function Wallet() {
    return (
        <AppLayout>
            Wallet
            <Routes>
                <Route index element={<Dashboard/>}/>
                <Route path="/bank" element={<Bank/>}/>
                <Route path="/*" element={<NotFound/>}/>
            </Routes>
        </AppLayout>
    );
}

export default Wallet;