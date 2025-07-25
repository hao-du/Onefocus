import React from 'react';
import { TransactionList } from './list/TransactionList';
import { TransactionProvider, CashFlowProvider } from './features';

export const TransactionIndex = React.memo(() => {
    return (
        <TransactionProvider>
            <CashFlowProvider>
                <TransactionList />
            </CashFlowProvider>
        </TransactionProvider>
    );
});