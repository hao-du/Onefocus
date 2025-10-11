import { Workspace } from '../../../../shared/components/layouts/workspace';
import { useCreateCounterparty, useGetAllCounterparties, useGetCounterpartyById, useUpdateCounterparty } from '../services';
import { Button } from '../../../../shared/components/controls';
import { Column, DataTable } from '../../../../shared/components/data/data-table';
import CounterpartyFormInput from './interfaces/CounterpartyFormInput';
import CounterpartyForm from './CounterpartyForm';
import React, { useCallback, useEffect, useState } from 'react';
import { useWindows } from '../../../../shared/components/hooks';
import { useSearchParams } from 'react-router';
import { CounterpartyResponse } from '../apis';

const CounterpartyList = React.memo(() => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [showForm, setShowForm] = useState(false);
    const { showResponseToast } = useWindows();

    const { entities: counterparties, isListLoading, refetch } = useGetAllCounterparties();
    const { onCreateAsync, isCreating } = useCreateCounterparty();
    const { onUpdateAsync, isUpdating } = useUpdateCounterparty();
    const { entity: selectedCounterparty, isEntityLoading, setCounterpartyId } = useGetCounterpartyById();

    const isPending = isListLoading || isEntityLoading || isCreating || isUpdating;

    const onSubmit = useCallback(async (data: CounterpartyFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                fullName: data.fullName ?? '',
                email: data.email,
                phoneNumber: data.phoneNumber,
                description: data.description
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setCounterpartyId(response.value.id);
            }
        }
        else {
            const response = await onUpdateAsync({
                id: data.id,
                fullName: data.fullName ?? '',
                email: data.email,
                phoneNumber: data.phoneNumber,
                description: data.description,
                isActive: data.isActive,
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                setShowForm(false);
            }
        }
        refetch();
    }, [onCreateAsync, onUpdateAsync, refetch, setCounterpartyId, showResponseToast]);

    useEffect(() => {
        const id = searchParams.get("id");
        if (id) {
            setCounterpartyId(id);
            setShowForm(true);
        }
    }, [searchParams, setCounterpartyId]);

    return (
        <Workspace
            title="Counterparties"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Add',
                    icon: 'pi pi-plus',
                    command: () => {
                        setCounterpartyId(undefined);
                        setShowForm(true);
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={counterparties} isPending={isPending} className="p-datatable-sm">
                        <Column field="fullName" header="Full name" className="w-2" />
                        <Column field="email" header="Email" className="w-3" />
                        <Column field="phoneNumber" header="Phone" className="w-2" />
                        <Column field="description" header="Description" className="w-auto" />
                        <Column className="w-1rem" body={(counterparty: CounterpartyResponse) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setCounterpartyId(counterparty.id);
                                    setShowForm(true);
                                    setSearchParams({ id: counterparty.id });
                                }}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm ? <CounterpartyForm selectedCounterparty={selectedCounterparty} isPending={isPending} onSubmit={onSubmit} /> : <></>
            }
        />
    );
});

export default CounterpartyList;