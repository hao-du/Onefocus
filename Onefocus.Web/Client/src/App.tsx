import {Route, Routes} from "react-router";
import Loading from "./presentation/pages/Loading";
import Home from "./presentation/pages/Home";
import Wallet from "./presentation/pages/Wallet";
import NotFound from "./presentation/pages/NotFound";
import useCheck from "./application/home/useCheck";
import AppLayout from "./presentation/layouts/AppLayout";

function App() {
    const {isCheckDone} = useCheck();
    if (!isCheckDone) {
        return <Loading/>;
    }

    return (
        <AppLayout>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="wallet/*" element={<Wallet/>}/>
                <Route path="*" element={<NotFound/>}/>
            </Routes>
        </AppLayout>
    );
}

export default App;
