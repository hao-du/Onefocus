import { useForm } from 'react-hook-form';
import SettingFormInput from './interfaces/SettingFormInput';
import { WorkspaceRightPanel } from '../../../../shared/components/layouts/workspace';
import { Switch, Text, Textarea } from '../../../../shared/components/controls';

type SettingFormProps = {
    setting: SettingFormInput | null | undefined;
    onSubmit: (data: SettingFormInput) => void;
    isPending?: boolean;
}

const BankForm = (props: SettingFormProps) => {
    const {control, handleSubmit} = useForm<SettingFormInput>({
        values: props.setting ? {...props.setting} :
            {
                locale: '',
                timezone: ''
            }
    });

    const isEditMode = Boolean(props.setting);

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

export default BankForm;