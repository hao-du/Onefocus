import { useEffect } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { USER_COMPONENT_NAMES } from "../../constants";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormPassword from "../../../shared/components/molecules/forms/FormPassword";
import useUpdatePassword from "./services/useUpdatePassword";

interface UpdatePasswordInput {
    id: string;
    password: string;
    confirmPassword: string;
}

const UpdatePassword = () => {
    const { isActiveComponent, closeComponent, dataId, setLoadings, hasAnyLoading } = usePage();
    const { showResponseToast } = useWindows();

    const { updatePasswordAsync, isUpdatingPassword } = useUpdatePassword();

    const { control, handleSubmit, getValues } = useForm<UpdatePasswordInput>({
        values: dataId ?
            {
                id: dataId,
                password: '',
                confirmPassword: ''
            } : {
                id: '',
                password: '',
                confirmPassword: ''
            }
    });

    useEffect(() => {
        setLoadings({ isUpdatingPassword });
    }, [isUpdatingPassword, setLoadings]);

    const onUpdatePassword = handleSubmit(async (data) => {
        const response = await updatePasswordAsync({
            id: data.id,
            password: data.password,
            confirmPassword: data.confirmPassword
        });
        showResponseToast(response, 'Updated password successfully.');
    });

    return (
        <Drawer
            title="Update Password"
            open={isActiveComponent(USER_COMPONENT_NAMES.UpdatePassword)}
            onClose={closeComponent}
            showPrimaryButton
            actions={[
                {
                    id: 'btnUpdatePassword',
                    label: 'Update',
                    command: onUpdatePassword,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <FormPassword control={control} name="password" label="Password" rules={{
                    required: 'Password is required.'
                }} />
                <FormPassword control={control} name="confirmPassword" label="Confirm Password" rules={{
                    required: 'Confirm Password is required.',
                    validate: (value) => value === getValues('password') || 'Passwords do not match.'
                }} />
            </Form>
        </Drawer>
    );
}
export default UpdatePassword;