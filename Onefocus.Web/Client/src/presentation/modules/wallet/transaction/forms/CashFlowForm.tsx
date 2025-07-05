import {useForm} from 'react-hook-form';
import {Switch, Textarea} from '../../../../components/form-controls';
import {CashFlow as DomainCashFlow} from '../../../../../domain/transactions/cashFlow';
import {CashFlowFormInput} from './CashFlowFormInput';
import {WorkspaceRightPanel} from '../../../../layouts/workspace';
import {Number} from '../../../../components/form-controls/inputs/Number';

export type CashFlowFormProps = {
    selectedCashFlow: DomainCashFlow | null | undefined;
    onSubmit: (data: CashFlowFormInput) => void;
    isPending?: boolean;
}

export const CashFlowForm = (props: CashFlowFormProps) => {
    const {control, handleSubmit} = useForm<CashFlowFormInput>({
        values: props.selectedCashFlow ? {...props.selectedCashFlow} :
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
                <Number control={control} name="amount" label="Amount" className="w-full of-w-max" rules={{
                    required: 'Amount is required.',
                    min: { value: 0, message: "Minimum amount is 0" },
                }}/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    required: 'Amount is required.',
                    maxLength: {value: 255, message: 'Name cannot exceed 255 characters.'}
                }}/>
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};