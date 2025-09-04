import React, { useCallback, useState } from 'react';
import { Button } from '../../../../../shared/components/controls';
import { Column, DataTable } from '../../../../../shared/components/data';
import { Workspace } from '../../../../../shared/components/layouts';
import { formatCurrency, formatDateLocalSystem } from '../../../../../shared/utils';
import { CurrencyResponse } from '../../../currency';
import { TransactionResponse } from '../../apis';
import { CashFlowForm, useCashFlow } from '../cashflow';
import { TransactionType } from './enums';
import { useTransactionList } from './hooks';
import { UniqueComponentId } from 'primereact/utils';
import { useTransactionPage } from '../../pages/hooks';

const TransactionList = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<TransactionType>(TransactionType.CashFlow);

    const { showForm, setShowForm } = useTransactionPage();
    const { transactions, currencies, isListLoading } = useTransactionList();
    const { selectedCashFlow, isCashFlowLoading, setTransactionIdFromCashFlow, onCashFlowSubmit } = useCashFlow();

    const isPending = isListLoading || isCashFlowLoading;

    const renderForm = useCallback((transactionType: TransactionType, currencies: CurrencyResponse[], isPending: boolean) => {
        //Make unique row ids for transaction items
        selectedCashFlow?.transactionItems?.map(item => {
            item.rowId = UniqueComponentId();
        });

        switch (transactionType) {
            case TransactionType.CashFlow:
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit} currencies={currencies} />
            case TransactionType.CurrencyExchange:
                break;
            case TransactionType.BankAccount:
                break;
            case TransactionType.PeerTransfer:
                break;
        }

        return null;
    }, [onCashFlowSubmit, selectedCashFlow])

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
                        setShowForm(true);
                        setTransactionIdFromCashFlow(null);
                        setSelectedTransactionType(TransactionType.CashFlow);
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
                    <Column body={(transaction: TransactionResponse) => {
                        switch (transaction.type) {
                            case TransactionType.CashFlow:
                                return (
                                    <Button
                                        icon="pi pi-pencil"
                                        className="p-button-text"
                                        onClick={() => {
                                            setTransactionIdFromCashFlow(transaction.id);
                                            setShowForm(true);
                                        }}
                                    />
                                );
                        }

                        return null;
                    }} header="" headerStyle={{ width: "4rem" }} />
                </DataTable>
            }
            rightPanel={
                showForm && renderForm(selectedTransactionType, currencies, isPending)
            }
        />
    );
});

export default TransactionList;