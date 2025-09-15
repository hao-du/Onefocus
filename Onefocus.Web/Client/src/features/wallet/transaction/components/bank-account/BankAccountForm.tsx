import { UniqueComponentId } from 'primereact/utils';
import { useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Option } from '../../../../../shared/components/controls';
import { Column, EditableTable } from '../../../../../shared/components/data/data-table';
import { DatePicker, Dropdown, Number, Switch, Text, Textarea } from '../../../../../shared/components/controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { getEmptyGuid } from '../../../../../shared/utils/formatUtils';
import { CurrencyResponse } from '../../../currency';
import BankAccountFormInput from './interfaces/BankAccountFormInput';
import { BankResponse } from '../../../bank';

type BankAccountFormProps = {
    selectedBankAccount: BankAccountFormInput | null | undefined;
    onSubmit: (data: BankAccountFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
    banks: BankResponse[]
}

const BankAccountForm = (props: BankAccountFormProps) => {
    const formValues = useMemo(() => {
        return props.selectedBankAccount ??
            {
                id: undefined,
                amount: 0,
                currencyId: getEmptyGuid(),
                interestRate: 1,
                accountNumber: '',
                issuedOn: new Date(),
                closedOn: undefined,
                isClosed: false,
                bankId: getEmptyGuid(),
                isActive: true,
                description: '',
                transactions: []
            }
    }, [props.selectedBankAccount]);

    const form = useForm<BankAccountFormInput>({
        defaultValues: formValues,
        values: formValues
    });

    const isEditMode = Boolean(props.selectedBankAccount);

    const currencyDropdownOptions = useMemo((): Option[] => {
        return props.currencies.map((currency) => {
            return {
                label: `${currency.shortName} - ${currency.name}`,
                value: currency.id
            } as Option;
        });
    }, [props.currencies])

    const bankDropdownOptions = useMemo((): Option[] => {
        return props.banks.map((bank) => {
            return {
                label: bank.name,
                value: bank.id
            } as Option;
        });
    }, [props.banks])

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
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} Bank Account`}</h3>
            <form key={props.selectedBankAccount?.id ?? 'new'}>
                <DatePicker control={form.control} name="issuedOn" label="Date" className="w-full of-w-max" rules={{
                    required: 'Issued On is required.',
                }} />
                <Dropdown control={form.control} name="bankId" label="Bank" className="w-full of-w-max" options={bankDropdownOptions} rules={{
                    validate: (value) => value && value !== getEmptyGuid() ? true : 'Bank is required.'
                }} />
                <Text control={form.control} name="accountNumber" label="Account Number" className="w-full of-w-max" rules={{
                    maxLength: { value: 50, message: 'Account Number cannot exceed 50 characters.' },
                }} />
                <Number control={form.control} name="interestRate" label="Interest Rate (%)" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Interest Rate is required.',
                    min: { value: 1, message: "Minimum interest rate is 1" },
                }} />
                <Number control={form.control} name="amount" label="Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0" },
                }} />
                <Switch control={form.control} name="isClosed" label="Is closed" description="Toggle between open and closed account." />
                {form.watch('isClosed') && <DatePicker control={form.control} name="closedOn" label="Closed On" className="w-full of-w-max" rules={{
                    required: 'Closed On is required when account is closed.',
                }} />}
                <Dropdown control={form.control} name="currencyId" label="Currency" className="w-full of-w-max" options={currencyDropdownOptions} rules={{
                    validate: (value) => value && value !== getEmptyGuid() ? true : 'Currency is required.',
                }} />
                <Textarea control={form.control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' },
                }} />
                {isEditMode && <Switch control={form.control} name="isActive" label="Is active" />}

                <EditableTable
                    isPending={props.isPending}
                    form={form}
                    path='transactions'
                    newRowValue={{
                        rowId: UniqueComponentId(),
                        transactedOn: new Date(),
                        amount: 0,
                        description: '',
                        isActive: true,
                    }}
                    tableName='Interests'
                    style={{ width: '40rem' }}
                >
                    <Column field="transactedOn" header="Transacted On" style={{ width: '15rem' }} editor={(options) => {
                        return (
                            <DatePicker
                                name={`transactions.${options.rowIndex}.transactedOn`}
                                control={form.control}
                                className="w-full"
                                rules={{
                                    required: 'Transacted On is required.'
                                }}
                                appendTo={document.body}
                            />);
                    }} />
                    <Column field="amount" header="Amount" style={{ width: '10rem' }} align="right" editor={(options) => {
                        return (
                            <Number
                                control={form.control}
                                name={`transactions.${options.rowIndex}.amount`}
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
                                name={`transactions.${options.rowIndex}.description`}
                                className="w-full"
                            />);
                    }} />
                </EditableTable>
            </form>
        </WorkspaceRightPanel>
    );
};

export default BankAccountForm;