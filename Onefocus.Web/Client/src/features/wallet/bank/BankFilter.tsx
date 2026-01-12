import { useForm } from "react-hook-form";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import GetBanksRequest from "../apis/interfaces/GetBanksRequest";
import { Form } from "antd";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";

interface BankFilterInput {
    name?: string | null,
    description?: string | null,
}

const BankFilter = () => {
    const {
        requestRefresh,
        currentComponentId,
        setCurrentComponentId,
        setFilter,
        isPageLoading
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

    return (
        <Drawer
            title="Filter"
            open={currentComponentId == BankFilter.id}
            onClose={() => setCurrentComponentId(undefined)}
            showPrimaryButton
            actions={[
                {
                    id: 'btnApplyFilter',
                    label: 'Apply',
                    command: onApply,
                    icon: <Icon name="apply" />,
                    isPending: isPageLoading()
                },
                {
                    id: 'btnResetFilter',
                    label: 'Reset',
                    command: () => {
                        reset();
                        setFilter({});
                        requestRefresh?.();
                    },
                    icon: <Icon name="reset" />,
                    isPending: isPageLoading()
                },
            ]}
        >
            <Form layout="vertical">
                <FormText name="name" control={control} label="Bank Name" className="w-full" />
                <FormTextArea name="description" control={control} label="Description" className="w-full" />
            </Form>
        </Drawer>
    );
}

BankFilter.id = 'BankFilter';
export default BankFilter;
