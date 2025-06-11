import {Workspace} from '../../../layouts/workspace';
import {useCreateCurrency, useGetAllCurrencies, useGetCurrencyById, useUpdateCurrency} from '../../../../application/currency';
import {Button} from '../../../components/controls/buttons';
import {Currency as DomainCurrency} from '../../../../domain/currency';
import {Column, DataTable} from '../../../components/data';
import {CurrencyFormInput} from './CurrencyFormInput';
import {CurrencyForm} from './CurrencyForm';
import React, {useCallback, useState} from 'react';
import {useWindows} from '../../../components/hooks/useWindows';

export const CurrencyIndex = React.memo(() => {
    const [showForm, setShowForm] = useState(false);
    const { showResponseToast } = useWindows();

    const { entities: currencies, isListLoading, refetch} = useGetAllCurrencies();
    const { onCreateAsync, isCreating } = useCreateCurrency();
    const { onUpdateAsync, isUpdating } = useUpdateCurrency();
    const { entity: selectedCurrency, isEntityLoading, setCurrencyId} = useGetCurrencyById();

    const isPending = isListLoading || isEntityLoading || isCreating || isUpdating;

    const onSubmit = useCallback(async (data: CurrencyFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                name: data.name!,
                shortName: data.shortName!,
                description: data.description,
                isDefault: data.isDefault
            });
            showResponseToast(response, 'Saved successfully.');
            if(response.status === 200 && response.value.id) {
                setCurrencyId(response.value.id);
            }
        }
        else {
            const response = await onUpdateAsync({
                id: data.id,
                name: data.name!,
                shortName: data.shortName!,
                isActive: data.isActive,
                isDefault: data.isDefault,
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
            title="Currencies"
            isPending={isPending}
            actionItems={[
                {
                    label: 'Add',
                    icon: 'pi pi-plus',
                    command: () => {
                        setCurrencyId(null);
                        setShowForm(true);
                    }
                }
            ]}
            leftPanel={
                <div className="overflow-auto flex-1">
                    <DataTable value={currencies} isPending={isPending} className="p-datatable-sm">
                        <Column field="name" header="Name" />
                        <Column field="shortName" header="Short name" />
                        <Column field="isDefault" header="Default" body={(rowData: DomainCurrency) => {
                            return rowData.isDefault ? <i className='pi pi-check-circle'></i> : <></>;
                        }}/>
                        <Column field="description" header="Description" />
                        <Column body={(currency: DomainCurrency) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setCurrencyId(currency.id);
                                    setShowForm(true);
                                }}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm ? <CurrencyForm selectedCurrency={selectedCurrency} isPending={isPending} onSubmit={onSubmit}/> : <></>
            }
        />
    );
});