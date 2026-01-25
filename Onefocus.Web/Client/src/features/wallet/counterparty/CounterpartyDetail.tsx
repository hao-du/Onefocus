import { useEffect } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { COUNTERPARTY_COMPONENT_NAMES } from "../../constants";
import useGetCounterpartyById from "./services/useGetCounterpartyById";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import useCreateCounterparty from "./services/useCreateCounterparty";
import useUpdateCounterparty from "./services/useUpdateCounterparty";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";

interface CounterpartyDetailInput {
    id?: string;
    fullName?: string;
    email?: string;
    phoneNumber?: string;
    isActive: boolean;
    description?: string;
}

const CounterpartyDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger } = usePage();
    const { showResponseToast } = useWindows();

    const { counterparty, isCounterpartyLoading } = useGetCounterpartyById(dataId);
    const { createAsync, isCreating } = useCreateCounterparty();
    const { updateAsync, isUpdating } = useUpdateCounterparty();

    const { control, handleSubmit } = useForm<CounterpartyDetailInput>({
        values: dataId && counterparty ? { ...counterparty } : {
            id: undefined,
            fullName: '',
            email: '',
            phoneNumber: '',
            isActive: true,
            description: ''
        }
    });

    useEffect(() => {
        setLoadings({ isCounterpartyLoading, isCreating, isUpdating });
    }, [isCounterpartyLoading, isCreating, isUpdating, setLoadings]);

    const onSave = handleSubmit(async (data) => {
        if (!data.id) {
            const response = await createAsync({
                fullName: data.fullName ?? '',
                email: data.email,
                phoneNumber: data.phoneNumber,
                description: data.description
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setDataId(response.value.id);
            }
        }
        else {
            const response = await updateAsync({
                id: data.id,
                fullName: data.fullName ?? '',
                email: data.email,
                phoneNumber: data.phoneNumber,
                description: data.description,
                isActive: data.isActive,
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                closeComponent();
            }
        }
    });

    return (
        <Drawer
            title={dataId ? 'Create' : 'Edit'}
            open={isActiveComponent(COUNTERPARTY_COMPONENT_NAMES.CounterpartyDetail)}
            onClose={closeComponent}
            showPrimaryButton
            expandDrawerTrigger={expandDrawerTrigger}
            actions={[
                {
                    id: 'btnSaveCounterparty',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <DrawerSection paddingTop>
                    <FormText control={control} name="fullName" label="Full name" rules={{
                        required: 'Full name is required.',
                        maxLength: { value: 100, message: 'Full name cannot exceed 100 characters.' }
                    }} />
                    <FormText control={control} name="email" label="Email" rules={{
                        maxLength: { value: 254, message: 'Email cannot exceed 254 characters.' },
                        pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: "Invalid email format" }
                    }} />
                    <FormText control={control} name="phoneNumber" label="Phone number" rules={{
                        maxLength: { value: 25, message: 'Phone cannot exceed 25 characters.' },
                        pattern: { value: /^[0-9+\-() ]+$/, message: "Only numbers and + - ( ) are allowed" },
                    }} />
                    <FormTextArea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                        maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' }
                    }} />
                    {dataId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
                </DrawerSection>
            </Form>
        </Drawer>
    );
}
export default CounterpartyDetail;