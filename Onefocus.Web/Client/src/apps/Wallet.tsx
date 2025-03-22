import {Route, Routes} from "react-router";

import Dashboard from "../wallet/modules/Dashboard";
import Bank from "../wallet/modules/Bank";
import NotFound from "./NotFound";

function Wallet() {
    return (
        <>
            Wallet
            <Routes>
                <Route index element={<Dashboard/>}/>
                <Route path="/bank" element={<Bank/>}/>
                <Route path="/*" element={<NotFound/>}/>
            </Routes>
        </>
    );
}

export default Wallet;