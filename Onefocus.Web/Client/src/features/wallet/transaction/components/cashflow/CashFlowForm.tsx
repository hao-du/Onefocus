import { UniqueComponentId } from 'primereact/utils';
import { useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Option } from '../../../../../shared/components/controls';
import { Column } from '../../../../../shared/components/data';
import { DatePicker, Dropdown, EditableTable, Number, Switch, Text, Textarea } from '../../../../../shared/components/form-controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { getEmptyGuid } from '../../../../../shared/utils/formatUtils';
import { CurrencyResponse } from '../../../currency';
import CashFlowFormInput from './interfaces/CashFlowFormInput';

type CashFlowFormProps = {
    selectedCashFlow: CashFlowFormInput | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
}

const CashFlowForm = (props: CashFlowFormProps) => {
    const form = useForm<CashFlowFormInput>({
        values: props.selectedCashFlow ? { ...props.selectedCashFlow } :
            {
                id: undefined,
                transactedOn: new Date(),
                amount: 0,
                currencyId: getEmptyGuid(),
                description: '',
                isIncome: true,
                isActive: true,
                transactionItems: []
            }
    });

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
                form.handleSubmit(props.onSubmit)();
            }
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} CashFlow`}</h3>
            <form>
                <DatePicker control={form.control} name="transactedOn" label="Date" className="w-full of-w-max" rules={{
                    required: 'Date is required.',
                }} />
                <Number control={form.control} name="amount" label="Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0" },
                }} />
                <Dropdown control={form.control} name="currencyId" label="Currency" className="w-full of-w-max" options={currencyDropdownOptions} rules={{
                    required: 'Currency is required.',
                }} />
                <Textarea control={form.control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' },
                }} />
                <Switch control={form.control} name="isIncome" label="Is income" description="Toggle between income and expense." />
                {isEditMode && <Switch control={form.control} name="isActive" label="Is active" />}

                <EditableTable
                    isPending={props.isPending}
                    form={form}
                    path='transactionItems'
                    newRowValue={{
                        rowId: UniqueComponentId(),
                        name: '',
                        amount: 0,
                        description: '',
                        isActive: true,
                    }}
                    tableName='Notes'
                    style={{ width: '40rem' }}
                >
                    <Column field="name" header="Name" style={{ width: '15rem' }} editor={(options) => {
                        return (
                            <Text
                                name={`transactionItems.${options.rowIndex}.name`}
                                control={form.control}
                                className="w-full"
                                rules={{
                                    required: 'Name is required.',
                                    maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                                }}
                            />);
                    }} />
                    <Column field="amount" header="Amount" style={{ width: '10rem' }} align="right" editor={(options) => {
                        return (
                            <Number
                                control={form.control}
                                name={`transactionItems.${options.rowIndex}.amount`}
                                inputClassName="w-full"
                                fractionDigits={2}
                                rules={{
                                    required: 'Amount is required.',
                                    min: { value: 0, message: "Minimum amount is 0" },
                                }} 
                            />);
                    }} />
                    <Column field="description" style={{ width: '15rem' }} header="Description" editor={(options) => {
                        return (
                            <Text
                                control={form.control}
                                name={`transactionItems.${options.rowIndex}.description`}
                                className="w-full"
                            />);
                    }} />
                </EditableTable>
            </form>
        </WorkspaceRightPanel>
    );
};

export default CashFlowForm;