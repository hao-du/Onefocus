import {Route, Routes} from "react-router";

import {useClient} from "../api/client/useClient.tsx";
import Loading from "../apps/Loading.tsx";
import Home from "../apps/Home.tsx";
import Wallet from "../apps/Wallet.tsx";
import NotFound from "../apps/NotFound.tsx";

function App() {
    const { isClientReady } = useClient();

    if(!isClientReady) {
        return <Loading />;
    }
    console.log("Client Ready");
    return (
        <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="wallet/*" element={<Wallet/>}/>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
}

export default App;
