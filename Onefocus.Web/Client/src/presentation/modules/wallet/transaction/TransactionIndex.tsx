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

export const TransactionIndex = React.memo(() => {
    const [selectedTransactionType, setSelectedTransactionType] = useState<TransactionType>(undefined);
    const [showForm, setShowForm] = useState(false);

    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();
    const { cashFlowEntity: selectedCashFlow, isCashFlowLoading, setTransactionId } = useGetCashFlowByTransactionId();

    const isPending = isListLoading || isCashFlowLoading;

    const onCashFlowSubmit = useCallback(async (data: CashFlowFormInput) => {
        console.log(data);
        refetch();
    }, []);

    const renderForm = useCallback((transactionType: TransactionType, isPending: boolean) => {
        switch (transactionType) {
            case 'CashFlow':
                return <CashFlowForm selectedCashFlow={selectedCashFlow} isPending={isPending} onSubmit={onCashFlowSubmit}/>
            case 'CurrencyExchange':
                break;
            case 'BankAccount':
                break;
            case  'PeerTransfer':
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
                    label: 'Actions',
                    icon: 'pi pi-plus',
                },
                {
                    label: 'Case flow',
                    icon: 'pi pi-money-bill',
                    command: () => {
                        setShowForm(true);
                        setSelectedTransactionType('CashFlow');
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
                showForm && renderForm(selectedTransactionType, isPending)
            }
        />
    );
});