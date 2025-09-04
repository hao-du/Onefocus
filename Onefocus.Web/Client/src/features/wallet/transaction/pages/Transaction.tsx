import React from 'react';
import { CashFlowProvider, TransactionList, TransactionListProvider } from '../components';
import { TransactionPageProvider } from './hooks';
import { BankAccountProvider } from '../components/bank-account';

const Transaction = React.memo(() => {
    return (
        <TransactionPageProvider>
            <TransactionListProvider>
                <CashFlowProvider>
                    <BankAccountProvider>
                        <TransactionList />
                    </BankAccountProvider>
                </CashFlowProvider>
            </TransactionListProvider>
        </TransactionPageProvider>
    );
});

export default Transaction;