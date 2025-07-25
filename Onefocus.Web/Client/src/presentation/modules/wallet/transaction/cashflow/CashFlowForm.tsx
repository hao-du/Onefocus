import { useFieldArray, useForm } from 'react-hook-form';
import { Switch, Textarea, Dropdown, DatePicker, Text } from '../../../../components/form-controls';
import { CashFlow as DomainCashFlow } from '../../../../../domain/transactions/cashFlow';
import { CashFlowFormInput } from './CashFlowFormInput';
import { WorkspaceRightPanel } from '../../../../layouts/workspace';
import { Number } from '../../../../components/form-controls/inputs/Number';
import { Currency } from '../../../../../domain/currency';
import { useMemo } from 'react';
import { DropdownOption } from '../../../../components/controls/inputs/Dropdown';
import { Column, DataTable } from '../../../../components/data';
import { Button } from '../../../../components/controls/buttons';

export type CashFlowFormProps = {
    selectedCashFlow: DomainCashFlow | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
    currencies: Currency[]
}

export const CashFlowForm = (props: CashFlowFormProps) => {
    const { control, handleSubmit } = useForm<CashFlowFormInput>({
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
    const { fields, append, remove } = useFieldArray({
        control,
        name: 'transactionItems',
    });

    const isEditMode = Boolean(props.selectedCashFlow);

    const currencyDropdownOptions = useMemo((): DropdownOption[] => {
        return props.currencies.map((currency) => {
            return {
                label: `${currency.shortName} - ${currency.name}`,
                value: currency.id
            } as DropdownOption;
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

                <DataTable value={fields} >
                    <Column header="Name" body={(_, { rowIndex }) => {
                        return (<Text control={control} name={`transactionItems.${rowIndex}.name`} label="Name" className="w-full of-w-max" rules={{
                            required: 'Name is required.',
                            maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                        }} />);
                    }} />
                    <Column header="Amount" body={(_, { rowIndex }) => {
                        return (<Number control={control} name={`transactionItems.${rowIndex}.amount`} label="Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                            required: 'Amount is required.',
                            min: { value: 0, message: "Minimum amount is 0" },
                        }} />);
                    }} />
                    <Column header="Description" body={(_, { rowIndex }) => {
                        return (<Textarea control={control} name={`transactionItems.${rowIndex}.description`} label="Description" className="w-full of-w-max" rules={{
                            maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' },
                        }} />);
                    }} />
                    <Column
                        header="Actions"
                        body={(_, { rowIndex }) => (
                            <Button
                                icon="pi pi-trash"
                                className="p-button-text p-button-danger"
                                onClick={() => remove(rowIndex)}
                            />
                        )}
                    />
                </DataTable>
            </form>
        </WorkspaceRightPanel>
    );
};