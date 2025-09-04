import React from 'react';
import { CashFlowProvider, TransactionList, TransactionListProvider } from '../components';
import { TransactionPageProvider } from './hooks';

const Transaction = React.memo(() => {
    return (
        <TransactionPageProvider>
            <TransactionListProvider>
                <CashFlowProvider>
                    <TransactionList />
                </CashFlowProvider>
            </TransactionListProvider>
        </TransactionPageProvider>
    );
});

export default Transaction;