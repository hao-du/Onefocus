import {Route, Routes} from "react-router";

import Dashboard from "../modules/wallet/Dashboard";
import Bank from "../modules/wallet/Bank";
import NotFound from "./NotFound";
import DefaultLayout from "../layouts/DefaultLayout";

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