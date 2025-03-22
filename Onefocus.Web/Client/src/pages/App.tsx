import {Route, Routes} from "react-router";

import Wallet from "../apps/Wallet";
import NotFound from "../apps/NotFound";

function App() {
    return (
        <Routes>
            <Route path="wallet/*" element={<Wallet/>}/>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
}

export default App;
