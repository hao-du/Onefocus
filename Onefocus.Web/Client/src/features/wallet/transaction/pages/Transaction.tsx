import React from 'react';
import { BankAccountProvider, CashFlowProvider, CurrencyExchangeProvider, PeerTransferProvider, TransactionList, TransactionListProvider } from '../components';
import { TransactionPageProvider } from './hooks';

const Transaction = React.memo(() => {
    return (
        <TransactionPageProvider>
            <TransactionListProvider>
                <CashFlowProvider>
                    <BankAccountProvider>
                        <PeerTransferProvider>
                            <CurrencyExchangeProvider>
                                <TransactionList />
                            </CurrencyExchangeProvider>
                        </PeerTransferProvider>
                    </BankAccountProvider>
                </CashFlowProvider>
            </TransactionListProvider>
        </TransactionPageProvider>
    );
});

export default Transaction;