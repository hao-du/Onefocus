import React, { useCallback, useState } from 'react';

import { Workspace } from '../../../../../shared/components/layouts';

import { TransactionResponse } from '../../apis';
import { TransactionType } from './enums';
import { useTransactionList } from './hooks';
import { useTransactionPage } from '../../pages/hooks';
import { CashFlowForm, useCashFlow } from '../cashflow';
import { BankAccountForm, useBankAccount } from '../bank-account';
import { CurrencyExchangeForm, useCurrencyExchange } from '../currency-exchange';
import { PeerTransferForm, usePeerTransfer } from '../peer-transfer';
import { DataView } from '../../../../../shared/components/data';
import { formatCurrency } from '../../../../../shared/utils';
import { Card } from '../../../../../shared/components/panel';
import { Button, Tag } from '../../../../../shared/components/controls';
import { useSettings } from '../../../../../shared/hooks';

const TransactionList = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<TransactionType>(TransactionType.CashFlow);

    const { formatDateTime } = useSettings();
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
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit} currencies={currencies} />

            case TransactionType.CurrencyExchange:
                return <CurrencyExchangeForm selectedCurrencyExchange={selectedCurrencyExchange} isPending={isPending} onSubmit={onCurrencyExchangeSubmit} currencies={currencies} />;

            case TransactionType.BankAccount:
                return <BankAccountForm selectedBankAccount={selectedBankAccount} isPending={isPending} onSubmit={onBankAccountSubmit} currencies={currencies} banks={banks} />;

            case TransactionType.PeerTransfer:
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
                <DataView
                    isPending={isPending}
                    className="overflow-x-hidden overflow-y-auto"
                    value={transactions}
                    listTemplate={(transactions: TransactionResponse[]) => {
                        if (!transactions || transactions.length === 0) return null;

                        const list = transactions.map((transaction, index) => {
                            const oddRow = index % 2 != 0;
                            return (
                                <Card key={index} className={`w-full ${!oddRow ? 'surface-background-color' : ''}`} index={index}>
                                    <div className="grid mt-0" key={index}>
                                        <div className="col-12 md:col-8 py-0">
                                            <div className="grid m-0">
                                                <div className="col-12 text-lg font-bold pb-1">
                                                    {formatDateTime(transaction.transactedOn, true)}
                                                </div>
                                                <div className="col-12 text-color-secondary font-italic text-sm py-1">
                                                    {transaction.description}
                                                </div>
                                                <div className="col-12 flex flex-wrap gap-2 py-1">
                                                    {transaction.tags?.map((value, index) => {
                                                        return <Tag key={index} value={value} className="text-xs"/>
                                                    })}
                                                </div>
                                            </div>
                                        </div>
                                        <div className="col-12 md:col-4 py-0">
                                            <div className="grid m-0 text-right">
                                                <div className="col-12 font-semibold text-sm pb-1">
                                                    {formatCurrency(transaction.amount)}
                                                </div>
                                                <div className="col-12 text-sm py-1">
                                                    {transaction.currencyName}
                                                </div>
                                                <div className="col-12 py-1">
                                                    <Button
                                                        icon="pi pi-pencil"
                                                        rounded
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
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </Card>
                            );
                        });

                        return <div className="grid">{list}</div>;
                    }}
                >
                </DataView>
            }
            rightPanel={
                showForm && renderForm(selectedTransactionType, isPending)
            }
        />
    );
});

export default TransactionList;