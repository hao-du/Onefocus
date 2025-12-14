import { useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { Option, TextOnly } from '../../../../../shared/components/controls';
import { DatePicker, Dropdown, Number, Switch, Textarea } from '../../../../../shared/components/controls';
import { WorkspaceRightPanel } from '../../../../../shared/components/layouts/workspace';
import { getEmptyGuid } from '../../../../../shared/utils/formatUtils';
import { CurrencyResponse } from '../../../currency';
import CurrencyExchangeFormInput from './interfaces/CurrencyExchangeFormInput';

type CurrencyExchangeFormProps = {
    selectedCurrencyExchange: CurrencyExchangeFormInput | null | undefined;
    onSubmit: (data: CurrencyExchangeFormInput) => void;
    isPending?: boolean;
    currencies: CurrencyResponse[]
}

const CurrencyExchangeForm = (props: CurrencyExchangeFormProps) => {
    const formValues = useMemo(() => {
        return props.selectedCurrencyExchange ??
        {
            id: undefined,
            sourceAmount: 0,
            sourceCurrencyId: getEmptyGuid(),
            targetAmount: 0,
            targetCurrencyId: getEmptyGuid(),
            exchangeRate: 1,
            transactedOn: new Date(),
            description: '',
            isActive: true,
        }
    }, [props.selectedCurrencyExchange]);

    const form = useForm<CurrencyExchangeFormInput>({
        defaultValues: formValues,
        values: formValues
    });

    const isEditMode = Boolean(props.selectedCurrencyExchange);

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
            <h3 className="mt-0 mb-5">
                <TextOnly value={`${isEditMode ? 'Edit' : 'Add'} CurrencyExchange`} />
            </h3>
            <form key={props.selectedCurrencyExchange?.id ?? 'new'}>
                <DatePicker control={form.control} name="transactedOn" label="Date" className="w-full of-w-max" rules={{
                    required: 'Date is required.',
                }} />
                <Number control={form.control} name="sourceAmount" label="Source Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Source Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0." },
                    max: { value: 10000000000, message: "Maximum amount is ten billion." },
                }} />
                <Dropdown control={form.control} name="sourceCurrencyId" label="Source Currency" className="w-full of-w-max" options={currencyDropdownOptions} rules={{
                    validate: {
                        required: (value) => value && value !== getEmptyGuid() ? true : 'Source Currency is required.'
                    }
                }} />

                <Number control={form.control} name="targetAmount" label="Target Amount" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Target Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0." },
                    max: { value: 10000000000, message: "Maximum amount is ten billion." },
                }} />
                <Dropdown control={form.control} name="targetCurrencyId" label="Target Currency" className="w-full of-w-max" options={currencyDropdownOptions} rules={{
                    validate: {
                        required: (value) => value && value !== getEmptyGuid() ? true : 'Target Currency is required.'
                    }
                }} />

                <Number control={form.control} name="exchangeRate" label="Rate (%)" className="w-full of-w-max" fractionDigits={2} rules={{
                    required: 'Rate is required.',
                    min: { value: 1, message: "Minimum exchange rate is 1." },
                    max: { value: 100, message: "Maximum exchange rate is 100." },
                }} />

                <Textarea control={form.control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                }} />

                {isEditMode && <Switch control={form.control} name="isActive" label="Is active" />}
            </form>
        </WorkspaceRightPanel>
    );
};

export default CurrencyExchangeForm;