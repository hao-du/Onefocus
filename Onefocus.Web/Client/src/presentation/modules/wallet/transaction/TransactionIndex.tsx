import {Workspace} from '../../../layouts/workspace';
import {Column, DataTable} from '../../../components/data';
import React, {useCallback, useState} from 'react';
import {useGetAllTransactions} from '../../../../application/transaction/useGetAllTransactions';
import {TransactionType} from './forms/CashFlowFrom.interface';
import {CashFlowForm} from './forms/CashFlowForm';
import {CashFlowFormInput} from './forms/CashFlowFormInput';
import {Transaction as DomainTransaction, TransactionType as DomainTransactionType} from '../../../../domain/transaction';
import {useGetCashFlowByTransactionId} from '../../../../application/transaction/useGetCashFlowByTransactionId';
import {Button} from '../../../components/controls/buttons';
import {useGetAllCurrencies} from '../../../../application/currency';
import {Currency} from '../../../../domain/currency';

export const TransactionIndex = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<TransactionType>(undefined);
    const [showForm, setShowForm] = useState(false);

    const { entities: currencies, isListLoading: isCurrenciesLoading} = useGetAllCurrencies();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();
    const { cashFlowEntity: selectedCashFlow, isCashFlowLoading, setTransactionId } = useGetCashFlowByTransactionId();

    const isPending = isListLoading || isCurrenciesLoading || isCashFlowLoading;

    const onCashFlowSubmit = useCallback(async (data: CashFlowFormInput) => {
        console.log(data);
        refetch();
    }, []);

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
                                            setTransactionId(transaction.id);
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