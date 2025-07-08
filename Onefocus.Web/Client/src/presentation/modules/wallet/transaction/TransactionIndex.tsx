import React from 'react';
import {TransactionList} from './list/TransactionList';
import {TransactionProvider} from './features/TransactionContext';
import {CashFlowProvider} from './features/CashFlowContext';

export const TransactionIndex = React.memo(() => {
    return (
        <TransactionProvider>
            <CashFlowProvider>
                <TransactionList />
            </CashFlowProvider>
        </TransactionProvider>
    );
});