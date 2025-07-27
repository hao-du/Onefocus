import React from 'react';
import { TransactionList } from '../list/TransactionList';
import { TransactionProvider, CashFlowProvider } from '../components';

const Transaction = React.memo(() => {
    return (
        <TransactionProvider>
            <CashFlowProvider>
                <TransactionList />
            </CashFlowProvider>
        </TransactionProvider>
    );
});

export default Transaction;