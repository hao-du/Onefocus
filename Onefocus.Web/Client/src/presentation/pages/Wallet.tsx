import {Route, Routes} from 'react-router';
import {NotFound} from '.';
import {BankIndex, CurrencyIndex, Dashboard} from '../modules/wallet';
import {TransactionIndex} from '../modules/wallet/transaction/TransactionIndex';

export function Wallet() {
    return (
        <Routes>
            <Route index element={<Dashboard/>}/>
            <Route path="/bank" element={<BankIndex/>}/>
            <Route path="/currency" element={<CurrencyIndex/>}/>
            <Route path="/transaction" element={<TransactionIndex/>}/>
            <Route path="/*" element={<NotFound/>}/>
        </Routes>
    );
}