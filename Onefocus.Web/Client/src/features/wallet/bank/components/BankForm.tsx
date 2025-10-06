import { useForm } from 'react-hook-form';
import BankFormInput from './interfaces/BankFormInput';
import { WorkspaceRightPanel } from '../../../../shared/components/layouts/workspace';
import { Switch, Text, Textarea, TextOnly } from '../../../../shared/components/controls';

type BankFormProps = {
    selectedBank: BankFormInput | null | undefined;
    onSubmit: (data: BankFormInput) => void;
    isPending?: boolean;
}

const BankForm = (props: BankFormProps) => {
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
            <h3 className="mt-0 mb-5">
                <TextOnly value={`${isEditMode ? 'Edit' : 'Add'} Bank`} />
            </h3>
            <form>
                <Text control={control} name="name" label="Name" className="w-full of-w-max" rules={{
                    required: 'Name is required.',
                    maxLength: {value: 100, message: 'Name cannot exceed 100 characters.'}
                }}/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: {value: 255, message: 'Description cannot exceed 255 characters.'}
                }}/>
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};

export default BankForm;