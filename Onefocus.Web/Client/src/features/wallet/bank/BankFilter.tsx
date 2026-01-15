import { useForm } from "react-hook-form";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import GetBanksRequest from "../apis/interfaces/bank/GetBanksRequest";
import FormText from "../../../shared/components/molecules/forms/FormText";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import { BANK_COMPONENT_NAMES } from "../../constants";
import { useCallback } from "react";
import Form from "../../../shared/components/molecules/forms/Form";
import FormNumber from "../../../shared/components/molecules/forms/FormNumber";
import FormDatePicker from "../../../shared/components/molecules/forms/FormDatePicker";
import dayjs, { Dayjs } from "dayjs";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";

interface BankFilterInput {
    name?: string | null,
    description?: string | null,
    amount?: number | null;
    startDate?: Dayjs | null;
    currencyId?: string | null;
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
            description: null,
            amount: 0,
            startDate: dayjs('2021/5/25'),
            currencyId: '2'
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
                <FormText name="name" control={control} label="Bank Name" />
                <FormTextArea name="description" control={control} label="Description" />
                <FormNumber name="amount" control={control} label="Amount" />
                <FormDatePicker name="startDate" control={control} label="Start Date" showTime />
                <FormSelect name="currencyId" control={control} label="Currency" options={[
                    {
                        label: 'VND',
                        value: '1'
                    },
                    {
                        label: 'AUS',
                        value: '2'
                    },
                    {
                        label: 'USD',
                        value: '3'
                    },
                    {
                        label: 'NND',
                        value: '4'
                    }

                ]} mode="tags" />
            </Form>
        </Drawer>
    );
}
export default BankFilter;
