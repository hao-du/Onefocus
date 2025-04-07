import {Route, Routes} from "react-router";
import Loading from "./presentation/pages/Loading";
import Home from "./presentation/pages/Home";
import Wallet from "./presentation/pages/Wallet";
import NotFound from "./presentation/pages/NotFound";
import {useClient} from "./infrastructure/hooks/client/useClient";

function App() {
    const {isClientReady} = useClient();

    if (!isClientReady) {
        return <Loading/>;
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
