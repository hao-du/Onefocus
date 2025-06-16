import {Workspace} from '../../../layouts/workspace';
import {Column, DataTable} from '../../../components/data';
import React, {useState} from 'react';
import {useGetAllTransactions} from '../../../../application/transaction/useGetAllTransactions';

export const TransactionIndex = React.memo(() => {
    const [showForm, setShowForm] = useState(false);

    const { entities: transactions, isListLoading } = useGetAllTransactions();

    const isPending = isListLoading;

    return (
        <Workspace
            title="Transactions"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Add',
                    icon: 'pi pi-plus',
                    command: () => {
                        //setTransactionId(null);
                        setShowForm(true);
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
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm ? <></> : <></>
            }
        />
    );
});