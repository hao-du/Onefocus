import { useForm } from 'react-hook-form';
import ChangePasswordFormInput from './interfaces/ChangePasswordFormInput';
import { WorkspaceRightPanel } from '../../../../shared/components/layouts/workspace';
import { Password, TextOnly } from '../../../../shared/components/controls';

type ChangePasswordFormProps = {
    selectedUser: ChangePasswordFormInput | null | undefined;
    onSubmit: (data: ChangePasswordFormInput) => void;
    isPending?: boolean;
}

const ChangePasswordForm = (props: ChangePasswordFormProps) => {
    const { control, handleSubmit, getValues } = useForm<ChangePasswordFormInput>({
        values: props.selectedUser ? { ...props.selectedUser } :
            {
                id: '',
                password: '',
                confirmPassword: ''
            }
    });

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
                <TextOnly value="Change password to " /> {props.selectedUser?.fullName ?? 'user'}
            </h3>
            <form autoComplete="off">
                <Password control={control} name="password" label="Password" className="w-full of-w-max" rules={{
                    required: 'Password is required.'
                }} />
                <Password control={control} name="confirmPassword" label="Confirm Password" className="w-full of-w-max" rules={{
                    required: 'Confirm Password is required.',
                    validate: (value) => value === getValues('password') || 'Passwords do not match.'
                }} />
            </form>
        </WorkspaceRightPanel>
    );
};

export default ChangePasswordForm;