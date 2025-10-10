import { useForm } from 'react-hook-form';
import UserFormInput from './interfaces/UserFormInput';
import { WorkspaceRightPanel } from '../../../../shared/components/layouts/workspace';
import { Password, Text, TextOnly } from '../../../../shared/components/controls';

type UserFormProps = {
    selectedUser: UserFormInput | null | undefined;
    onSubmit: (data: UserFormInput) => void;
    isPending?: boolean;
}

const UserForm = (props: UserFormProps) => {
    const { control, handleSubmit } = useForm<UserFormInput>({
        values: props.selectedUser ? { ...props.selectedUser } :
            {
                id: undefined,
                email: '',
                firstName: '',
                lastName: '',
            }
    });

    const isEditMode = Boolean(props.selectedUser);

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
                <TextOnly value={`${isEditMode ? 'Edit' : 'Add'} User`} />
            </h3>
            <form autoComplete="off">
                <Text control={control} name="email" label="Email" className="w-full of-w-max" rules={{
                    required: 'Email is required.',
                    maxLength: { value: 256, message: 'Email cannot exceed 256 characters.' }
                }} />
                <Text control={control} name="firstName" label="First name" className="w-full of-w-max" rules={{
                    required: 'First name is required.',
                    maxLength: { value: 50, message: 'First name cannot exceed 50 characters.' }
                }} />
                <Text control={control} name="lastName" label="Last name" className="w-full of-w-max" rules={{
                    required: 'Last name is required.',
                    maxLength: { value: 50, message: 'Last name cannot exceed 50 characters.' }
                }} />
                {!isEditMode && (
                    <Password name="password" control={control} label="Password" className="w-full" autoComplete="new-password" rules={{
                        required: 'Password is required.'
                    }} />
                )}
            </form>
        </WorkspaceRightPanel>
    );
};

export default UserForm;