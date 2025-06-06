import {useForm} from 'react-hook-form';
import {Switch, Text, Textarea} from '../../../components/form-controls';
import {Bank as DomainBank} from '../../../../domain/bank';
import {BankFormInput} from './BankFormInput';
import {WorkspaceRightPanel} from '../../../layouts/workspace';

export type BankFormProps = {
    selectedBank: DomainBank | null | undefined;
    onSubmit: (data: BankFormInput) => void;
    isPending?: boolean;
}

export const BankForm = (props: BankFormProps) => {
    const {control, handleSubmit} = useForm<BankFormInput>({
        values: props.selectedBank ? {...props.selectedBank} :
            {
                id: undefined,
                name: '',
                isActive: false,
                description: '',
            }
    });

    const isEditMode = Boolean(props.selectedBank);

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
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} Bank`}</h3>
            <form>
                <Text control={control} name="name" label="Name" className="w-full of-w-max"/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max"/>
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};