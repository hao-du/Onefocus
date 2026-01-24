import { useEffect } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { USER_COMPONENT_NAMES } from "../../constants";
import useGetUserById from "./services/useGetUserById";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import FormText from "../../../shared/components/molecules/forms/FormText";
import useCreateUser from "./services/useCreateUser";
import useUpdateUser from "./services/useUpdateUser";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormPassword from "../../../shared/components/molecules/forms/FormPassword";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";

interface UserDetailInput {
    id?: string
    email: string;
    firstName: string;
    lastName: string;
    password?: string;
}

const UserDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger } = usePage();
    const { showResponseToast } = useWindows();

    const { user, isUserLoading } = useGetUserById(dataId);
    const { createAsync, isCreating } = useCreateUser();
    const { updateAsync, isUpdating } = useUpdateUser();

    const { control, handleSubmit } = useForm<UserDetailInput>({
        values: dataId && user ? { ...user } : {
            id: undefined,
            email: '',
            firstName: '',
            lastName: '',
        }
    });

    useEffect(() => {
        setLoadings({ isUserLoading, isCreating, isUpdating });
    }, [isUserLoading, isCreating, isUpdating, setLoadings]);

    const onSave = handleSubmit(async (data) => {
        if (!data.id) {
            const response = await createAsync({
                email: data.email,
                firstName: data.firstName,
                lastName: data.lastName,
                password: data.password ?? ''
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setDataId(response.value.id);
            }
        }
        else {
            const response = await updateAsync({
                id: data.id,
                firstName: data.firstName,
                lastName: data.lastName,
                email: data.email
            });
            showResponseToast(response, 'Updated successfully.');
        }
    });

    return (
        <Drawer
            title={dataId ? 'Create' : 'Edit'}
            open={isActiveComponent(USER_COMPONENT_NAMES.UserDetail)}
            onClose={closeComponent}
            showPrimaryButton
            expandDrawerTrigger={expandDrawerTrigger}
            actions={[
                {
                    id: 'btnSaveUser',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <DrawerSection paddingTop>
                    <FormText control={control} name="email" label="Email" rules={{
                        required: 'Email is required.',
                        maxLength: { value: 256, message: 'Email cannot exceed 256 characters.' }
                    }} />
                    <FormText control={control} name="firstName" label="First name" rules={{
                        required: 'First name is required.',
                        maxLength: { value: 50, message: 'First name cannot exceed 50 characters.' }
                    }} />
                    <FormText control={control} name="lastName" label="Last name" rules={{
                        required: 'Last name is required.',
                        maxLength: { value: 50, message: 'Last name cannot exceed 50 characters.' }
                    }} />
                    {!dataId && (
                        <FormPassword control={control} name="password" label="Password" autoComplete="new-password" rules={{
                            required: 'Password is required.'
                        }} />
                    )}
                </DrawerSection>
            </Form>
        </Drawer>
    );
}
export default UserDetail;