import React from 'react';
import { CashFlowProvider, TransactionList, TransactionProvider } from '../components';

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