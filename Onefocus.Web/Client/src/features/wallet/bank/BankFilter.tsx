import { useForm } from "react-hook-form";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import GetBanksRequest from "../apis/interfaces/GetBanksRequest";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import { BANK_COMPONENT_NAMES } from "../../constants";
import { useCallback } from "react";
import Form from "../../../shared/components/molecules/forms/Form";

interface BankFilterInput {
    name?: string | null,
    description?: string | null,
}

const BankFilter = () => {
    const {
        requestRefresh,
        isActiveComponent,
        closeComponent,
        setFilter,
        resetFilter,
        hasAnyLoading
    } = usePage();

    const { control, handleSubmit, reset } = useForm<BankFilterInput>({
        defaultValues: {
            name: null,
            description: null
        }
    });

    const onApply = handleSubmit((data) => {
        setFilter({
            name: data.name,
            description: data.description
        } as GetBanksRequest);
        requestRefresh?.();
    });

    const onReset = useCallback(() => {
        reset();
        resetFilter();
        requestRefresh?.();
    }, [requestRefresh, reset, resetFilter]);

    return (
        <Drawer
            title="Filter"
            open={isActiveComponent(BANK_COMPONENT_NAMES.BankFilter)}
            onClose={closeComponent}
            showPrimaryButton
            actions={[
                {
                    id: 'btnApplyFilter',
                    label: 'Apply',
                    command: onApply,
                    icon: <Icon name="apply" />,
                    isPending: hasAnyLoading
                },
                {
                    id: 'btnResetFilter',
                    label: 'Reset',
                    command: onReset,
                    icon: <Icon name="reset" />,
                    isPending: hasAnyLoading
                },
            ]}
        >
            <Form>
                <FormText name="name" control={control} label="Bank Name" className="w-full" />
                <FormTextArea name="description" control={control} label="Description" className="w-full" />
            </Form>
        </Drawer>
    );
}
export default BankFilter;
