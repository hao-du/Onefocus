import {Route, Routes} from 'react-router';
import useCheck from './application/home/useCheck';
import {AppLayout} from './presentation/layouts';
import {Home, Loading, NotFound, Wallet} from './presentation/pages';

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
