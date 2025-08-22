import { UniqueComponentId } from 'primereact/utils';
import { useMemo, useState } from 'react';
import { useFieldArray, useForm } from 'react-hook-form';
import { Option } from '../../../../../shared/components/controls';
import { Column, DataTable } from '../../../../../shared/components/data';
import { DatePicker, Dropdown, Number, Switch, Text, Textarea } from '../../../../../shared/components/form-controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { CurrencyResponse } from '../../../currency';
import CashFlowFormInput from './interfaces/CashFlowFormInput';
import TransactionItemFormInput from './interfaces/TransactionItemFormInput';

type CashFlowFormProps = {
    selectedCashFlow: CashFlowFormInput | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
}

const CashFlowForm = (props: CashFlowFormProps) => {
    const { control, getValues, handleSubmit, watch } = useForm<CashFlowFormInput>({
        defaultValues: props.selectedCashFlow ? { ...props.selectedCashFlow } :
            {
                id: undefined,
                transactedOn: new Date(),
                amount: undefined,
                currencyId: undefined,
                description: '',
                isIncome: true,
                isActive: true,
                transactionItems: []
            }
    });
    const { fields, update, append, remove } = useFieldArray({
        control,
        name: 'transactionItems'
    });
    const watchedItems = watch('transactionItems');
    const [editingRows, setEditingRows] = useState<Record<string, boolean>>({});
    const [clonedRows, setClonedRows] = useState<Record<string, TransactionItemFormInput>>({});

    const isEditMode = Boolean(props.selectedCashFlow);

    const currencyDropdownOptions = useMemo((): Option[] => {
        return props.currencies.map((currency) => {
            return {
                label: `${currency.shortName} - ${currency.name}`,
                value: currency.id
            } as Option;
        });
    }, [props.currencies])

    const buttons = [
        {
            id: 'btnSave',
            label: 'Save',
            icon: 'pi pi-save',
            onClick: () => {
                handleSubmit(props.onSubmit)();
            }
        },
        {
            id: 'btnAddItem',
            label: 'Add Item',
            icon: 'pi pi-plus',
            onClick: () => {
                append({
                    rowId: UniqueComponentId(),
                    name: '',
                    amount: 0,
                    isActive: true,
                })
            }
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} CashFlow`}</h3>
            <form>
                <DatePicker control={control} name="transactedOn" label="Date" className="w-full of-w-max" rules={{
                    required: 'Date is required.',
                }} />
                <Number control={control} name="amount" label="Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0" },
                }} />
                <Dropdown control={control} name="currencyId" label="Currency" className="w-full of-w-max" options={currencyDropdownOptions} rules={{
                    required: 'Currency is required.',
                }} />
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' },
                }} />
                <Switch control={control} name="isIncome" label="Is income" description="Toggle between income and expense." />
                {isEditMode && <Switch control={control} name="isActive" label="Is active" />}

                 <DataTable
                    value={watchedItems}
                    editMode="row"
                    dataKey="rowId"
                    isPending={props.isPending}
                    editingRows={editingRows}
                    onRowEditInit={(event) => {
                        const rowId = event.data.rowId;
                        const currentValues = getValues();
                        const currentRow = currentValues.transactionItems[event.index];

                        setClonedRows((prev) => ({
                            ...prev,
                            [rowId]: { ...currentRow }
                        }));
                        setEditingRows((prev) => ({
                            ...prev,
                            [rowId]: true
                        }));
                    }}
                    onRowEditCancel={(event) => {
                        const rowId = event.data.rowId;

                        if (clonedRows[rowId]) {
                            update(event.index, { ...clonedRows[rowId] });
                        }

                        setEditingRows((prev) => {
                            const updated = { ...prev };
                            delete updated[rowId];
                            return updated;
                        });

                        setClonedRows((prev) => {
                            const updated = { ...prev };
                            delete updated[rowId];
                            return updated;
                        });
                    }}
                    onRowEditSave={(event) => {
                        const rowId = event.data.rowId;
                        const currentValues = getValues();
                        const updatedRow = currentValues.transactionItems[event.index];

                        update(event.index, updatedRow);

                        setEditingRows((prev) => {
                            const updated = { ...prev };
                            delete updated[rowId];
                            return updated;
                        });

                        setClonedRows((prev) => {
                            const updated = { ...prev };
                            delete updated[rowId];
                            return updated;
                        });
                    }}
                >
                    <Column header="Name" editor={(options) => {
                        return (
                            <Text
                                name={`transactionItems.${options.rowIndex}.name`}
                                control={control}
                                label="Name"
                                className="w-full of-w-max"
                                rules={{
                                    required: 'Name is required.',
                                    maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                                }}
                            />);
                    }} />
                    <Column
                        header="Actions"
                        rowEditor
                        headerStyle={{ width: '10%', minWidth: '8rem' }}
                        bodyStyle={{ textAlign: 'center' }}
                    />
                </DataTable>
            </form>
        </WorkspaceRightPanel>
    );
};

export default CashFlowForm;