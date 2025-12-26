import React, { useCallback, useEffect, useState } from 'react';

import { Workspace } from '../../../../../shared/components/layouts';

import { TransactionResponse } from '../../apis';
import { FormType } from './enums';
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
import { useLocale } from '../../../../../shared/hooks';
import { useSearchTransactions } from '../../services';
import SearchForm from '../search/SearchForm';
import { useSearchParams } from 'react-router';

const TransactionList = React.memo(() => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [selectedFormType, setSelectedFormType] = useState<FormType>(FormType.CashFlow);

    const { formatDateTime } = useLocale();
    const { showForm, setShowForm } = useTransactionPage();

    const { transactions, currencies, banks, counterparties, isListLoading } = useTransactionList();
    const { onSearchAsync, isSearching, searchCriteria, setSearchCriteria } = useSearchTransactions();

    const { selectedCashFlow, isCashFlowLoading, setCashFlowTransactionId, onCashFlowSubmit } = useCashFlow();
    const { selectedBankAccount, isBankAccountLoading, setBankAccountTransactionId, onBankAccountSubmit } = useBankAccount();
    const { selectedCurrencyExchange, isCurrencyExchangeLoading, setCurrencyExchangeTransactionId, onCurrencyExchangeSubmit } = useCurrencyExchange();
    const { selectedPeerTransfer, isPeerTransferLoading, setPeerTransferTransactionId, onPeerTransferSubmit } = usePeerTransfer();

    const isPending = isListLoading || isCashFlowLoading || isBankAccountLoading || isCurrencyExchangeLoading || isPeerTransferLoading || isSearching;

    const renderForm = useCallback(() => {
        switch (selectedFormType) {
            case FormType.CashFlow:
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit} currencies={currencies} />

            case FormType.CurrencyExchange:
                return <CurrencyExchangeForm selectedCurrencyExchange={selectedCurrencyExchange} isPending={isPending} onSubmit={onCurrencyExchangeSubmit} currencies={currencies} />;

            case FormType.BankAccount:
                return <BankAccountForm selectedBankAccount={selectedBankAccount} isPending={isPending} onSubmit={onBankAccountSubmit} currencies={currencies} banks={banks} />;

            case FormType.PeerTransfer:
                return <PeerTransferForm selectedPeerTransfer={selectedPeerTransfer} isPending={isPending} onSubmit={onPeerTransferSubmit} currencies={currencies} counterparties={counterparties} />;

            case FormType.Search:
                return <SearchForm onSearch={onSearchAsync} isPending={isPending} searchCriteria={searchCriteria} setSearchCriteria={setSearchCriteria} />
        }

    }, [selectedFormType, selectedCashFlow, isPending, onCashFlowSubmit, currencies, selectedCurrencyExchange, onCurrencyExchangeSubmit, selectedBankAccount, onBankAccountSubmit, banks, selectedPeerTransfer, onPeerTransferSubmit, counterparties, onSearchAsync, searchCriteria, setSearchCriteria]);

    useEffect(() => {
        const paramId = searchParams.get("id");
        if (!paramId) return;

        const paramType = searchParams.get("type");
        const type = paramType !== null ? (Number(paramType)) : undefined;
        if (type == null || type == undefined) return;
        
        setSelectedFormType(type);
        switch (type) {
            case FormType.CashFlow:
                setCashFlowTransactionId(paramId);
                break;
            case FormType.CurrencyExchange:
                setCurrencyExchangeTransactionId(paramId);
                break;
            case FormType.BankAccount:
                setBankAccountTransactionId(paramId);
                break;
            case FormType.PeerTransfer:
                setPeerTransferTransactionId(paramId);
                break;
            case FormType.Search:
                break;
        }
        setShowForm(true);
    }, [searchParams, setBankAccountTransactionId, setCashFlowTransactionId, setCurrencyExchangeTransactionId, setPeerTransferTransactionId, setShowForm]);

    return (
        <Workspace
            title="Transactions"
            isPending={isPending}
            actionItems={[
                {
                    icon: 'pi pi-search',
                    severity: 'info',
                    command: () => {
                        setSelectedFormType(FormType.Search);
                        setShowForm(true);
                    }
                },
                {
                    label: 'Actions...',
                    items: [
                        {
                            label: 'Cash flow',
                            icon: 'pi pi-money-bill',
                            command: () => {
                                setSelectedFormType(FormType.CashFlow);
                                setCashFlowTransactionId(null);
                                setShowForm(true);
                            }
                        },
                        {
                            label: 'Bank account',
                            icon: 'pi pi-building-columns',
                            command: () => {
                                setSelectedFormType(FormType.BankAccount);
                                setBankAccountTransactionId(null);
                                setShowForm(true);
                            }
                        },
                        {
                            label: 'Exchange',
                            icon: 'pi pi-arrow-right-arrow-left',
                            command: () => {
                                setSelectedFormType(FormType.CurrencyExchange);
                                setCurrencyExchangeTransactionId(null);
                                setShowForm(true);
                            }
                        },
                        {
                            label: 'Peer transfer',
                            icon: 'pi pi-users',
                            command: () => {
                                setSelectedFormType(FormType.PeerTransfer);
                                setPeerTransferTransactionId(null);
                                setShowForm(true);
                            }
                        }
                    ],
                },
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
                                                        return <Tag key={index} value={value} className="text-xs" />
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
                                                                case FormType.CashFlow:
                                                                    setSelectedFormType(FormType.CashFlow);
                                                                    setCashFlowTransactionId(transaction.id);
                                                                    break;
                                                                case FormType.BankAccount:
                                                                    setSelectedFormType(FormType.BankAccount);
                                                                    setBankAccountTransactionId(transaction.id);
                                                                    break;
                                                                case FormType.CurrencyExchange:
                                                                    setSelectedFormType(FormType.CurrencyExchange);
                                                                    setCurrencyExchangeTransactionId(transaction.id);
                                                                    break;
                                                                case FormType.PeerTransfer:
                                                                    setSelectedFormType(FormType.PeerTransfer);
                                                                    setPeerTransferTransactionId(transaction.id);
                                                                    break;
                                                            }

                                                            setSearchParams({ id: transaction.id, type: String(transaction.type) });
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
                showForm && renderForm()
            }
        />
    );
});

export default TransactionList;