import {Workspace} from '../../../layouts/workspace';
import {useCreateBank, useGetAllBanks, useGetBankById, useUpdateBank} from '../../../../application/bank';
import {Button} from '../../../components/controls/buttons';
import {Bank as DomainBank} from '../../../../domain/bank';
import {Column, DataTable} from '../../../components/data';
import {BankFormInput} from './BankFormInput';
import {BankForm} from './BankForm';
import React, {useCallback, useState} from 'react';
import {useWindows} from '../../../components/hooks/useWindows';

export const BankIndex = React.memo(() => {
    const [showForm, setShowForm] = useState(false);
    const { showResponseToast } = useWindows();

    const {entities: banks, isListLoading, refetch} = useGetAllBanks();
    const { onCreateAsync, isCreating } = useCreateBank();
    const { onUpdateAsync, isUpdating } = useUpdateBank();
    const {entity: selectedBank, isEntityLoading, setBankId} = useGetBankById();

    const isPending = isListLoading || isEntityLoading || isCreating || isUpdating;

    const onSubmit = useCallback(async (data: BankFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                name: data.name ?? '',
                description: data.description
            });
            showResponseToast(response, 'Saved successfully.');
            if(response.status === 200 && response.value.id) {
                setBankId(response.value.id);
            }
        }
        else {
            const response = await onUpdateAsync({
                id: data.id,
                name: data.name ?? '',
                isActive: data.isActive,
                description: data.description
            });
            showResponseToast(response, 'Updated successfully.');
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
                        setBankId(null);
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
                                    setBankId(bank.id);
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