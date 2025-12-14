import { Route, Routes } from 'react-router';
import { NotFound } from '../../shared/app';
import { Bank } from './bank';
import { Currency } from './currency';
import { Dashboard } from './dashboard/Dashboard';
import { Transaction } from './transaction';
import { Counterparty } from './counterparty';

const WalletRoutes = () => {
    return (
        <Routes>
            <Route index element={<Dashboard />} />
            <Route path="/bank" element={<Bank />} />
            <Route path="/currency" element={<Currency />} />
            <Route path="/transaction" element={<Transaction />} />
            <Route path="/counterparty" element={<Counterparty />} />
            <Route path="/*" element={<NotFound />} />
        </Routes>
    );
}

export default WalletRoutes;