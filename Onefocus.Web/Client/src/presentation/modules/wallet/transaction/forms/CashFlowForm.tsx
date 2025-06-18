import {useForm} from 'react-hook-form';
import {Switch, Text, Textarea} from '../../../../components/form-controls';
import {CashFlow as DomainCashFlow} from '../../../../../domain/bank';
import {CashFlowFormInput} from './../CashFlowFormInput';
import {WorkspaceRightPanel} from '../../../../layouts/workspace';

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
                name: '',
                isActive: false,
                description: '',
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
                <Text control={control} name="name" label="Name" className="w-full of-w-max" rules={{
                    required: 'Name is required.',
                    maxLength: {value: 100, message: 'Name cannot exceed 100 characters.'}
                }}/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: {value: 255, message: 'Name cannot exceed 255 characters.'}
                }}/>
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};