import { useEffect } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { BANK_COMPONENT_NAMES } from "../../constants";
import useGetBankById from "./services/useGetBankById";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import useCreateBank from "./services/useCreateBank";
import useUpdateBank from "./services/useUpdateBank";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";

interface BankDetailInput {
    id?: string
    name?: string;
    isActive: boolean;
    description?: string;
}

const BankDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, requestRefresh } = usePage();
    const { showResponseToast } = useWindows();

    const { bank, isBankLoading } = useGetBankById(dataId);
    const { createAsync, isCreating } = useCreateBank();
    const { updateAsync, isUpdating } = useUpdateBank();

    const { control, handleSubmit } = useForm<BankDetailInput>({
        values: dataId && bank ? { ...bank } : {
            id: undefined,
            name: '',
            isActive: true,
            description: '',
        }
    });

    useEffect(() => {
        setLoadings({ isBankLoading, isCreating, isUpdating });
    }, [isBankLoading, isCreating, isUpdating, setLoadings]);

    const onSave = handleSubmit(async (data) => {
        if (!data.id) {
            const response = await createAsync({
                name: data.name ?? '',
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
                name: data.name ?? '',
                isActive: data.isActive,
                description: data.description
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                closeComponent();
            }
        }
        requestRefresh?.();
    });

    return (
        <Drawer
            title={dataId ? 'Create' : 'Edit'}
            open={isActiveComponent(BANK_COMPONENT_NAMES.BankDetail)}
            onClose={closeComponent}
            showPrimaryButton
            actions={[
                {
                    id: 'btnSaveBank',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <FormText name="name" control={control} label="Bank Name" rules={{
                    required: 'Name is required.',
                    maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                }} />
                <FormTextArea name="description" control={control} label="Description" rules={{
                    maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' }
                }} />
                {dataId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
            </Form>
        </Drawer>
    );
}
export default BankDetail;