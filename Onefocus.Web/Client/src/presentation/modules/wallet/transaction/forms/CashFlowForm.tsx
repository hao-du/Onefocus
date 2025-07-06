import {useForm} from 'react-hook-form';
import {Switch, Textarea, Dropdown, DatePicker} from '../../../../components/form-controls';
import {CashFlow as DomainCashFlow} from '../../../../../domain/transactions/cashFlow';
import {CashFlowFormInput} from './CashFlowFormInput';
import {WorkspaceRightPanel} from '../../../../layouts/workspace';
import {Number} from '../../../../components/form-controls/inputs/Number';
import {Currency} from '../../../../../domain/currency';
import {useMemo} from 'react';
import {DropdownOption} from '../../../../components/controls/inputs/Dropdown';

export type CashFlowFormProps = {
    selectedCashFlow: DomainCashFlow | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
    currencies: Currency[]
}

export const CashFlowForm = (props: CashFlowFormProps) => {
    const {control, handleSubmit} = useForm<CashFlowFormInput>({
        defaultValues: props.selectedCashFlow ? {...props.selectedCashFlow} :
            {
                id: undefined,
                transactedOn: new Date(),
                amount: undefined,
                currencyId: undefined,
                description: '',
                isIncome: true,
                isActive: true,
            }
    });

    const isEditMode = Boolean(props.selectedCashFlow);

    const currencyDropdownOptions = useMemo(():DropdownOption[] => {
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
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} CashFlow`}</h3>
            <form>
                <DatePicker control={control} name="transactedOn" label="Date" className="w-full of-w-max" rules={{
                    required: 'Date is required.',
                }}/>
                <Number control={control} name="amount" label="Amount" className="w-full of-w-max" fractionDigits="2" rules={{
                    required: 'Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0" },
                }}/>
                <Dropdown control={control} name="currencyId" label="Currency" className="w-full of-w-max"  options={currencyDropdownOptions} rules={{
                    required: 'Currency is required.',
                }}/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: {value: 255, message: 'Name cannot exceed 255 characters.'},
                }}/>
                <Switch control={control} name="isIncome" label="Is income" description="Toggle between income and expense."/>
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};