import { useEffect, useMemo } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import usePage from "../../../shared/hooks/page/usePage";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import Icon from "../../../shared/components/atoms/misc/Icon";
import Form from "../../../shared/components/molecules/forms/Form";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import { useForm } from "react-hook-form";
import useWindows from "../../../shared/hooks/windows/useWindows";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";
import { getCurrencyOptions, TransactionItemInput } from "./shared";
import useGetCashFlowByTransactionId from "./services/cashflow/useGetCashFlowByTransactionId";
import useCreateCashFlow from "./services/cashflow/useCreateCashFlow";
import useUpdateCashFlow from "./services/cashflow/useUpdateCashFlow";
import { getEmptyGuid } from "../../../shared/utils";
import useGetAllCurrencies from "../currency/services/useGetAllCurrencies";
import FormDatePicker from "../../../shared/components/molecules/forms/FormDatePicker";
import FormNumber from "../../../shared/components/molecules/forms/FormNumber";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";
import dayjs, { Dayjs } from "dayjs";
import FormRepeater from "../../../shared/components/molecules/forms/FormRepeater";
import FormText from "../../../shared/components/molecules/forms/FormText";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";

interface CashFlowDetailInput {
    id?: string;
    transactedOn: Dayjs;
    amount: number;
    currencyId: string;
    description?: string;
    isIncome: boolean;
    isActive: boolean;
    transactionItems: TransactionItemInput[];
}

const CashFlowDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger: triggerSignal } = usePage();
    const { showResponseToast } = useWindows();

    const { cashFlow, isCashFlowLoading, refetchCashFlow } = useGetCashFlowByTransactionId(dataId);
    const { currencies, isCurrenciesLoading } = useGetAllCurrencies();
    const { createCashFlowAsync, isCashFlowCreating } = useCreateCashFlow();
    const { updateCashFlowAsync, isCashFlowUpdating } = useUpdateCashFlow();

    const formValues = useMemo<CashFlowDetailInput>(() => {
        return dataId && cashFlow ? {
            ...cashFlow,
            transactedOn: dayjs(cashFlow.transactedOn)
        } : {
            id: undefined,
            transactedOn: dayjs(new Date()),
            amount: 0,
            currencyId: getEmptyGuid(),
            description: '',
            isIncome: true,
            isActive: true,
            transactionItems: []
        }
    }, [dataId, cashFlow]);

    const form = useForm<CashFlowDetailInput>({
        defaultValues: formValues,
        values: formValues
    });
    const { control, handleSubmit } = form;

    useEffect(() => {
        setLoadings({ isCashFlowLoading, isCashFlowCreating, isCashFlowUpdating, isCurrenciesLoading });
    }, [setLoadings, isCashFlowLoading, isCashFlowCreating, isCashFlowUpdating, isCurrenciesLoading]);

    const onSave = handleSubmit(async (data: CashFlowDetailInput) => {
        if (!data.id) {
            const response = await createCashFlowAsync({
                amount: data.amount,
                currencyId: data.currencyId,
                isIncome: data.isIncome,
                transactedOn: data.transactedOn.toDate(),
                description: data.description,
                transactionItems: data.transactionItems.map(item => ({
                    id: item.id,
                    name: item.name,
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
                }))
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setDataId(response.value.id);
            }
        } else {
            const response = await updateCashFlowAsync({
                id: data.id,
                amount: data.amount,
                currencyId: data.currencyId,
                isIncome: data.isIncome,
                isActive: data.isActive,
                transactedOn: data.transactedOn.toDate(),
                description: data.description,
                transactionItems: data.transactionItems.map(item => ({
                    id: item.id,
                    name: item.name,
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
                }))
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                closeComponent();
                return;
            }
            refetchCashFlow();
        }
    });

    return (
        <Drawer
            title={`${!dataId ? 'Create' : 'Edit'} Cashflow`}
            open={isActiveComponent(TRANSACTION_COMPONENT_NAMES.CashFlow)}
            onClose={closeComponent}
            showPrimaryButton
            expandDrawerTrigger={triggerSignal}
            actions={[
                {
                    id: 'btnSaveCashFlow',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <DrawerSection paddingTop>
                    <FormDatePicker focus showTime control={control} name="transactedOn" label="Date" rules={{
                        required: 'Date is required.',
                    }} />
                    <FormNumber control={control} name="amount" label="Amount" formatted precision={2} rules={{
                        required: 'Amount is required.',
                        min: { value: 0, message: "Minimum amount is 0." },
                        max: { value: 10000000000, message: "Maximum amount is ten billion (10,000,000,000)." },
                    }} />
                    <FormSelect control={control} name="currencyId" label="Currency" options={getCurrencyOptions(currencies)} rules={{
                        validate: {
                            required: (value) => value && value !== getEmptyGuid() ? true : 'Currency is required.'
                        }
                    }} />
                    <FormTextArea control={control} name="description" label="Description" rules={{
                        maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                    }} />
                    <FormSwitch control={control} name="isIncome" checkedLabel="Income" uncheckedLabel="Expense" extra="Toggle between income and expense." />
                    {dataId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
                </DrawerSection>
                <FormRepeater
                    title="Notes"
                    form={form}
                    path='transactionItems'
                    defaultRowValue={{
                        name: '',
                        amount: 0,
                        description: '',
                        isActive: true,
                    }}
                    render={(_, control, index, isReadMode, isFocused) => {
                        return (
                            <div>
                                <FormText readOnly={isReadMode} focus={isFocused} control={control} label="Item Name" name={`transactionItems.${index}.name`} rules={{
                                    required: 'Item name is required.',
                                    maxLength: { value: 100, message: 'Item name cannot exceed 100 characters.' }
                                }} />
                                <FormNumber readOnly={isReadMode} control={control} label="Amount" formatted precision={2} name={`transactionItems.${index}.amount`} rules={{
                                    required: 'Amount is required.',
                                    min: { value: 0, message: "Minimum amount is 0." },
                                    max: { value: 10000000000, message: "Maximum amount is ten billion." },
                                }} />
                                <FormTextArea readOnly={isReadMode} control={control} label="Description" rows={2} name={`transactionItems.${index}.description`} rules={{
                                    maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                                }} />
                            </div>
                        );
                    }}
                />
            </Form>
        </Drawer>
    );
}
export default CashFlowDetail;