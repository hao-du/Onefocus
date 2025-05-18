import {Route, Routes} from 'react-router';
import {NotFound} from '.';
import {Bank, Dashboard} from '../modules/wallet';
import {Currency} from '../modules/wallet/Currency';

export function Wallet() {
    return (
        <Routes>
            <Route index element={<Dashboard/>}/>
            <Route path="/bank" element={<Bank/>}/>
            <Route path="/currency" element={<Currency/>}/>
            <Route path="/*" element={<NotFound/>}/>
        </Routes>
    );
}