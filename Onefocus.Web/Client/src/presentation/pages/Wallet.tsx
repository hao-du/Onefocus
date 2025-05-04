import {Route, Routes} from 'react-router';
import {NotFound} from '.';
import {Bank, Dashboard} from '../modules/wallet';

export function Wallet() {
    return (
        <Routes>
            <Route index element={<Dashboard/>}/>
            <Route path="/bank" element={<Bank/>}/>
            <Route path="/*" element={<NotFound/>}/>
        </Routes>
    );
}