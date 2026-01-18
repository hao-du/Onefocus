import { useEffect } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { CURRENCY_COMPONENT_NAMES } from "../../constants";
import useGetCurrencyById from "./services/useGetCurrencyById";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import useCreateCurrency from "./services/useCreateCurrency";
import useUpdateCurrency from "./services/useUpdateCurrency";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";

interface CurrencyDetailInput {
    id?: string
    name?: string;
    shortName?: string;
    isActive: boolean;
    isDefault: boolean;
    description?: string;
}

const CurrencyDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, requestRefresh } = usePage();
    const { showResponseToast } = useWindows();

    const { currency, isCurrencyLoading } = useGetCurrencyById(dataId);
    const { createAsync, isCreating } = useCreateCurrency();
    const { updateAsync, isUpdating } = useUpdateCurrency();

    const { control, handleSubmit } = useForm<CurrencyDetailInput>({
        values: dataId && currency ? { ...currency } : {
            id: undefined,
            name: '',
            shortName: '',
            isActive: false,
            isDefault: false,
            description: '',
        }
    });

    useEffect(() => {
        setLoadings({ isCurrencyLoading, isCreating, isUpdating });
    }, [isCurrencyLoading, isCreating, isUpdating, setLoadings]);

    const onSave = handleSubmit(async (data) => {
        if (!data.id) {
            const response = await createAsync({
                name: data.name ?? '',
                shortName: data.shortName ?? '',
                description: data.description,
                isDefault: data.isDefault
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
                shortName: data.shortName ?? '',
                isActive: data.isActive,
                isDefault: data.isDefault,
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
            open={isActiveComponent(CURRENCY_COMPONENT_NAMES.CurrencyDetail)}
            onClose={closeComponent}
            showPrimaryButton
            actions={[
                {
                    id: 'btnSaveCurrency',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <FormText control={control} name="name" label="Name" className="w-full of-w-max" rules={{
                    required: 'Name is required.',
                    maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                }} />
                <FormText control={control} name="shortName" label="Short name" className="w-full of-w-max" rules={{
                    required: 'Short name is required.',
                    minLength: { value: 3, message: 'Short name cannot less than 3 characters.' },
                    maxLength: { value: 4, message: 'Short name cannot exceed 4 characters.' }
                }} />
                <FormTextArea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Name cannot exceed 255 characters.' }
                }} />
                {<FormSwitch control={control} name="isDefault" label="Set as Default" extra="Enabling this flag will automatically disable it for all other currencies." />}
                {dataId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
            </Form>
        </Drawer>
    );
}
export default CurrencyDetail;