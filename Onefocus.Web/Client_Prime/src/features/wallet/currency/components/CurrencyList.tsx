import React, { useCallback, useEffect, useState } from 'react';
import { Button } from '../../../../shared/components/controls';
import { Column, DataTable } from '../../../../shared/components/data/data-table';
import { useWindows } from '../../../../shared/components/hooks';
import { Workspace } from '../../../../shared/components/layouts/workspace';
import { CurrencyResponse } from '../apis';
import { useCreateCurrency, useGetAllCurrencies, useGetCurrencyById, useUpdateCurrency } from '../services';
import CurrencyForm from './CurrencyForm';
import CurrencyFormInput from './interfaces/CurrencyFormInput';
import { useSearchParams } from 'react-router';

const CurrencyList = React.memo(() => {
    const [searchParams, setSearchParams] = useSearchParams();

    const [showForm, setShowForm] = useState(false);
    const { showResponseToast } = useWindows();

    const { entities: currencies, isListLoading, refetch } = useGetAllCurrencies();
    const { onCreateAsync, isCreating } = useCreateCurrency();
    const { onUpdateAsync, isUpdating } = useUpdateCurrency();
    const { entity: selectedCurrency, isEntityLoading, setCurrencyId } = useGetCurrencyById();

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
            if (response.status === 200 && response.value.id) {
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
            if (!data.isActive) {
                setShowForm(false);
            }
        }
        refetch();
    }, [onCreateAsync, onUpdateAsync, refetch, setCurrencyId, showResponseToast]);

    useEffect(() => {
        const id = searchParams.get("id");
        if(id){
            setCurrencyId(id);
            setShowForm(true);
        }
    }, [searchParams, setCurrencyId]);

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
                        <Column field="name" header="Name" className="w-3" />
                        <Column field="shortName" header="Short name" className="w-1" />
                        <Column field="isDefault" header="Default" className="w-1 text-center" body={(rowData: CurrencyResponse) => {
                            return rowData.isDefault ? <i className='pi pi-check-square text-success'></i> : <></>;
                        }} />
                        <Column field="description" header="Description" className="w-auto" />
                        <Column className="w-1rem" body={(currency: CurrencyResponse) => (
                            <Button
                                icon="pi pi-pencil"
                                className="p-button-text"
                                onClick={() => {
                                    setCurrencyId(currency.id);
                                    setShowForm(true);
                                    setSearchParams({ id: currency.id });
                                }}
                            />
                        )} header="" headerStyle={{ width: "4rem" }} />
                    </DataTable>
                </div>
            }
            rightPanel={
                showForm ? <CurrencyForm selectedCurrency={selectedCurrency} isPending={isPending} onSubmit={onSubmit} /> : <></>
            }
        />
    );
});

export default CurrencyList;