import { useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Option } from '../../../../../shared/components/controls';
import { DatePicker, Dropdown, Number, Switch, Text, Textarea } from '../../../../../shared/components/controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { getEmptyGuid } from '../../../../../shared/utils/formatUtils';
import { CurrencyResponse } from '../../../currency';
import CashFlowFormInput from './interfaces/CashFlowFormInput';
import { EditableDataView } from '../../../../../shared/components/data';

type CashFlowFormProps = {
    selectedCashFlow: CashFlowFormInput | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
}

const CashFlowForm = (props: CashFlowFormProps) => {
    const formValues = useMemo(() => {
        return props.selectedCashFlow ??
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
    }, [props.selectedCashFlow]);

    const form = useForm<CashFlowFormInput>({
        defaultValues: formValues,
        values: formValues
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
            <form key={props.selectedCashFlow?.id ?? 'new'}>
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

                <EditableDataView
                    headerName='Notes'
                    form={form}
                    path='transactionItems'
                    isPending={props.isPending}
                    newRowValue={{
                        name: '',
                        amount: 0,
                        description: '',
                        isActive: true,
                    }}
                    inputs={[
                        (index, isEditing) => <Text
                            textOnly={!isEditing}
                            label='Item name'
                            name={`transactionItems.${index}.name`}
                            control={form.control}
                            className="w-full of-w-max"
                            size='small'
                            rules={{
                                required: 'Name is required.',
                                maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                            }}
                        />,
                        (index, isEditing) => <Number
                            textOnly={!isEditing}
                            label='Amount'
                            control={form.control}
                            name={`transactionItems.${index}.amount`}
                            inputClassName="w-full of-w-max"
                            size='small'
                            fractionDigits={2}
                            rules={{
                                required: 'Amount is required.',
                                min: { value: 0, message: "Minimum amount is 0" },
                            }}
                        />,
                        (index, isEditing) => <Text
                            textOnly={!isEditing}
                            label='Description'
                            control={form.control}
                            size='small'
                            name={`transactionItems.${index}.description`}
                            className="w-full of-w-max"
                        />
                    ]}
                />
            </form>
        </WorkspaceRightPanel>
    );
};

export default CashFlowForm;