import {Workspace} from '../../../layouts/workspace';
import {useCreateBank, useGetAllBanks, useGetBankById, useUpdateBank} from '../../../../application/bank';
import {Button} from '../../../components/controls/buttons';
import {Bank as DomainBank} from '../../../../domain/bank';
import {Column, DataTable} from '../../../components/data';
import {BankFormInput} from './BankFormInput';
import {BankForm} from './BankForm';
import React, {useCallback, useState} from 'react';

export const BankIndex = React.memo(() => {
    const [showForm, setShowForm] = useState(false);

    const {banks, isListLoading, refetch} = useGetAllBanks();
    const { onCreateAsync, isCreating } = useCreateBank();
    const { onUpdateAsync, isUpdating } = useUpdateBank();
    const {bank: selectedBank, isEntityLoading, setbankId} = useGetBankById();

    const isPending = isListLoading || isEntityLoading || isCreating || isUpdating;

    const onSubmit = useCallback(async (data: BankFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                name: data.name ?? '',
                description: data.description
            });
            if(response.status === 200 && response.value.id) {
                setbankId(response.value.id)
            }
        }
        else {
            await onUpdateAsync({
                id: data.id,
                name: data.name ?? '',
                isActive: data.isActive,
                description: data.description
            });
            if(!data.isActive) {
                setShowForm(false);
            }
        }
        refetch();
    }, []);

    return (
        <Workspace
            title="Banks"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Add',
                    icon: 'pi pi-plus',
                    command: () => {
                        setbankId(null);
                        setShowForm(true);
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={banks} isPending={isPending} className="p-datatable-sm">
                        <Column field="name" header="Name" />
                        <Column field="description" header="Description" />
                        <Column body={(bank: DomainBank) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setbankId(bank.id);
                                    setShowForm(true);
                                }}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm ? <BankForm selectedBank={selectedBank} isPending={isPending} onSubmit={onSubmit}/> : <></>
            }
        />
    );
});