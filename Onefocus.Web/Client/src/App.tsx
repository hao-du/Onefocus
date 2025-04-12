import {Route, Routes} from "react-router";
import Loading from "./presentation/pages/Loading";
import Home from "./presentation/pages/Home";
import Wallet from "./presentation/pages/Wallet";
import NotFound from "./presentation/pages/NotFound";
import useCheck from "./application/home/useCheck";

function App() {
    const {isCheckDone} = useCheck();
    if (!isCheckDone) {
        return <Loading/>;
    }

    return (
        <Routes>
            <Route path="/" element={<Home/>}/>
            <Route path="wallet/*" element={<Wallet/>}/>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
}

export default App;
