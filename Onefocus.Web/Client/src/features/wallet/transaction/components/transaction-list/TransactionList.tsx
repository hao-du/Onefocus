import React, { useCallback, useState } from 'react';
import { UniqueComponentId } from 'primereact/utils';

import { formatCurrency, formatDateLocalSystem } from '../../../../../shared/utils';
import { Button } from '../../../../../shared/components/controls';
import { Column, DataTable } from '../../../../../shared/components/data';
import { Workspace } from '../../../../../shared/components/layouts';

import { TransactionResponse } from '../../apis';
import { TransactionType } from './enums';
import { useTransactionList } from './hooks';
import { useTransactionPage } from '../../pages/hooks';
import { CashFlowForm, useCashFlow } from '../cashflow';
import { BankAccountForm, useBankAccount } from '../bank-account';
import { CurrencyExchangeForm, useCurrencyExchange } from '../currency-exchange';
import { PeerTransferForm, usePeerTransfer } from '../peer-transfer';

const TransactionList = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<TransactionType>(TransactionType.CashFlow);

    const { showForm, setShowForm } = useTransactionPage();
    const { transactions, currencies, banks, counterparties, isListLoading } = useTransactionList();
    const { selectedCashFlow, isCashFlowLoading, setCashFlowTransactionId, onCashFlowSubmit } = useCashFlow();
    const { selectedBankAccount, isBankAccountLoading, setBankAccountTransactionId, onBankAccountSubmit } = useBankAccount();
    const { selectedCurrencyExchange, isCurrencyExchangeLoading, setCurrencyExchangeTransactionId, onCurrencyExchangeSubmit } = useCurrencyExchange();
    const { selectedPeerTransfer, isPeerTransferLoading, setPeerTransferTransactionId, onPeerTransferSubmit } = usePeerTransfer();

    const isPending = isListLoading || isCashFlowLoading || isBankAccountLoading || isCurrencyExchangeLoading || isPeerTransferLoading;

    const renderForm = useCallback((transactionType: TransactionType, isPending: boolean) => {
        switch (transactionType) {
            case TransactionType.CashFlow:
                //Make unique row ids for transaction items
                selectedCashFlow?.transactionItems?.map(item => {
                    item.rowId = UniqueComponentId();
                });
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit} currencies={currencies} />

            case TransactionType.CurrencyExchange:
                return <CurrencyExchangeForm selectedCurrencyExchange={selectedCurrencyExchange} isPending={isPending} onSubmit={onCurrencyExchangeSubmit} currencies={currencies} />;

            case TransactionType.BankAccount:
                //Make unique row ids for transactions
                selectedBankAccount?.transactions?.map(item => {
                    item.rowId = UniqueComponentId();
                });
                return <BankAccountForm selectedBankAccount={selectedBankAccount} isPending={isPending} onSubmit={onBankAccountSubmit} currencies={currencies} banks={banks} />;

            case TransactionType.PeerTransfer:
                //Make unique row ids for transactions
                selectedPeerTransfer?.transferTransactions?.map(item => {
                    item.rowId = UniqueComponentId();
                });
                return <PeerTransferForm selectedPeerTransfer={selectedPeerTransfer} isPending={isPending} onSubmit={onPeerTransferSubmit} currencies={currencies} counterparties={counterparties} />;

        }

    }, [
        banks, 
        counterparties, 
        currencies, 
        onBankAccountSubmit, 
        onCashFlowSubmit, 
        onCurrencyExchangeSubmit, 
        onPeerTransferSubmit, 
        selectedBankAccount, 
        selectedCashFlow, 
        selectedCurrencyExchange, 
        selectedPeerTransfer
    ]);

    return (
        <Workspace
            title="Transactions"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Actions...',
                },
                {
                    label: 'Cash flow',
                    icon: 'pi pi-money-bill',
                    command: () => {
                        setSelectedTransactionType(TransactionType.CashFlow);
                        setCashFlowTransactionId(null);
                        setShowForm(true);
                    }
                },
                {
                    label: 'Bank account',
                    icon: 'pi pi-building-columns',
                    command: () => {
                        setSelectedTransactionType(TransactionType.BankAccount);
                        setBankAccountTransactionId(null);
                        setShowForm(true);
                    }
                },
                {
                    label: 'Exchange',
                    icon: 'pi pi-arrow-right-arrow-left',
                    command: () => {
                        setSelectedTransactionType(TransactionType.CurrencyExchange);
                        setCurrencyExchangeTransactionId(null);
                        setShowForm(true);
                    }
                },
                {
                    label: 'Peer transfer',
                    icon: 'pi pi-users',
                    command: () => {
                        setSelectedTransactionType(TransactionType.PeerTransfer);
                        setPeerTransferTransactionId(null);
                        setShowForm(true);
                    }
                }
            ]}
            leftPanel={
                <DataTable value={transactions} isPending={isPending} className="p-datatable-sm">
                    <Column header="Date" body={(transaction: TransactionResponse) => {
                        return formatDateLocalSystem(transaction.transactedOn);
                    }} />
                    <Column header="Amount" align="right" alignHeader="right" body={(transaction: TransactionResponse) => {
                        return formatCurrency(transaction.amount);
                    }} />
                    <Column field="currencyName" header="Currency" />
                    <Column field="description" header="Description" />
                    <Column header="" headerStyle={{ width: "4rem" }} body={(transaction: TransactionResponse) => (
                        <Button
                            icon="pi pi-pencil"
                            className="p-button-text"
                            onClick={() => {
                                switch (transaction.type) {
                                    case TransactionType.CashFlow:
                                        setSelectedTransactionType(TransactionType.CashFlow);
                                        setCashFlowTransactionId(transaction.id);
                                        break;
                                    case TransactionType.BankAccount:
                                        setSelectedTransactionType(TransactionType.BankAccount);
                                        setBankAccountTransactionId(transaction.id);
                                        break;
                                    case TransactionType.CurrencyExchange:
                                        setSelectedTransactionType(TransactionType.CurrencyExchange);
                                        setCurrencyExchangeTransactionId(transaction.id);
                                        break;
                                    case TransactionType.PeerTransfer:
                                        setSelectedTransactionType(TransactionType.PeerTransfer);
                                        setPeerTransferTransactionId(transaction.id);
                                        break;
                                }
                                setShowForm(true);
                            }} />
                    )} />
                </DataTable>
            }
            rightPanel={
                showForm && renderForm(selectedTransactionType, isPending)
            }
        />
    );
});

export default TransactionList;