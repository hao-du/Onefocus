import React, {useCallback, useState} from 'react';
import {
    Transaction as DomainTransaction,
    TransactionType,
    TransactionType as DomainTransactionType
} from '../../../../domain/transaction';
import {Button} from '../../../../components/controls/buttons';
import {CashFlowForm} from '../cashflow/CashFlowForm';
import {Column, DataTable} from '../../../../components/data';
import {Workspace} from '../../../../shared/components/layouts/workspace';
import {Currency} from '../../../../domain/currency';
import {useCashFlow, useTransaction} from '../components';

export const TransactionList = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<DomainTransactionType>(TransactionType.CashFlow);
    const [showForm, setShowForm] = useState(false);

    const { transactions, currencies, isListLoading } = useTransaction();
    const { selectedCashFlow, isCashFlowLoading, setTransactionIdFromCashFlow, onCashFlowSubmit } = useCashFlow();

    const isPending = isListLoading || isCashFlowLoading;

    const renderForm = useCallback((transactionType: DomainTransactionType, currencies: Currency[], isPending: boolean) => {
        switch (transactionType) {
            case DomainTransactionType.CashFlow:
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit} currencies={currencies}/>
            case DomainTransactionType.CurrencyExchange:
                break;
            case DomainTransactionType.BankAccount:
                break;
            case DomainTransactionType.PeerTransfer:
                break;
        }

        return <></>;
    }, [])

    return (
        <Workspace
            title="Transactions"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Actions...',
                },
                {
                    label: 'Case flow',
                    icon: 'pi pi-money-bill',
                    command: () => {
                        setShowForm(true);
                        setSelectedTransactionType(DomainTransactionType.CashFlow);
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={transactions} isPending={isPending} className="p-datatable-sm">
                        <Column field="transactedOn" header="Date" />
                        <Column field="amount" header="Amount" />
                        <Column field="currencyName" header="Currency" />
                        <Column field="description" header="Description" />
                        <Column body={(transaction: DomainTransaction) => {
                            switch (transaction.type) {
                                case DomainTransactionType.CashFlow:
                                    <Button
                                        icon="pi pi-pencil"
                                        className="p-button-text"
                                        onClick={() => {
                                            setTransactionIdFromCashFlow(transaction.id);
                                            setShowForm(true);
                                        }}
                                    />
                                    break;
                            }

                            return <></>
                        }} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm && renderForm(selectedTransactionType, currencies, isPending)
            }
        />
    );
});