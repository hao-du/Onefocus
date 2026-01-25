import dayjs, { Dayjs } from "dayjs";
import usePage from "../../../shared/hooks/page/usePage";
import useWindows from "../../../shared/hooks/windows/useWindows";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import useGetAllCurrencies from "../currency/services/useGetAllCurrencies";
import useCreateCurrencyExchange from "./services/currency-exchange/useCreateCurrencyExchange";
import useGetCurrencyExchangeByTransactionId from "./services/currency-exchange/useGetCurrencyExchangeByTransactionId";
import useUpdateCurrencyExchange from "./services/currency-exchange/useUpdateCurrencyExchange";
import { useEffect, useMemo } from "react";
import { useForm } from "react-hook-form";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import Form from "../../../shared/components/molecules/forms/Form";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";
import FormDatePicker from "../../../shared/components/molecules/forms/FormDatePicker";
import FormNumber from "../../../shared/components/molecules/forms/FormNumber";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import Icon from "../../../shared/components/atoms/misc/Icon";
import { getCurrencyOptions } from "./shared";
import { getEmptyGuid } from "../../../shared/utils";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";

interface CurrencyExchangeDetailInput {
    id?: string;
    sourceAmount: number;
    sourceCurrencyId: string;
    targetAmount: number;
    targetCurrencyId: string;
    exchangeRate: number;
    transactedOn: Dayjs;
    description?: string;
    isActive: boolean;
}

const CurrencyExchangeDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger } = usePage();

    const isActive = isActiveComponent(TRANSACTION_COMPONENT_NAMES.CurrencyExchange);
    const transactionId = isActive ? dataId : undefined;

    const { showResponseToast } = useWindows();

    const { currencyExchange, isCurrencyExchangeLoading } = useGetCurrencyExchangeByTransactionId(transactionId);
    const { currencies, isCurrenciesLoading } = useGetAllCurrencies();
    const { createCurrencyExchangeAsync, isCurrencyExchangeCreating } = useCreateCurrencyExchange();
    const { updateCurrencyExchangeAsync, isCurrencyExchangeUpdating } = useUpdateCurrencyExchange();

    const formValues = useMemo<CurrencyExchangeDetailInput>(() => {
        return transactionId && currencyExchange ? {
            ...currencyExchange,
            transactedOn: dayjs(currencyExchange.transactedOn)
        } : {
            id: undefined,
            sourceAmount: 0,
            sourceCurrencyId: getEmptyGuid(),
            targetAmount: 0,
            targetCurrencyId: getEmptyGuid(),
            exchangeRate: 1,
            transactedOn: dayjs(new Date()),
            description: '',
            isActive: true,
        }
    }, [transactionId, currencyExchange]);

    const form = useForm<CurrencyExchangeDetailInput>({
        defaultValues: formValues,
        values: formValues
    });
    const { control, handleSubmit } = form;

    useEffect(() => {
        setLoadings({ isCurrencyExchangeLoading, isCurrenciesLoading, isCurrencyExchangeCreating, isCurrencyExchangeUpdating });
    }, [setLoadings, isCurrencyExchangeLoading, isCurrenciesLoading, isCurrencyExchangeCreating, isCurrencyExchangeUpdating]);

    const onSave = handleSubmit(async (data: CurrencyExchangeDetailInput) => {
        if (!data.id) {
            const response = await createCurrencyExchangeAsync({
                sourceAmount: data.sourceAmount,
                sourceCurrencyId: data.sourceCurrencyId,
                targetAmount: data.targetAmount,
                targetCurrencyId: data.targetCurrencyId,
                exchangeRate: data.exchangeRate,
                transactedOn: data.transactedOn.toDate(),
                description: data.description,
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setDataId(response.value.id);
            }
        } else {
            const response = await updateCurrencyExchangeAsync({
                id: data.id,
                sourceAmount: data.sourceAmount,
                sourceCurrencyId: data.sourceCurrencyId,
                targetAmount: data.targetAmount,
                targetCurrencyId: data.targetCurrencyId,
                exchangeRate: data.exchangeRate,
                transactedOn: data.transactedOn.toDate(),
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
            title={`${!transactionId ? 'Create' : 'Edit'} Cashflow`}
            open={isActive}
            onClose={closeComponent}
            showPrimaryButton
            expandDrawerTrigger={expandDrawerTrigger}
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
                    <FormNumber control={control} name="sourceAmount" label="Source Amount" formatted precision={2} rules={{
                        required: 'Source Amount is required.',
                        min: { value: 0, message: "Minimum amount is 0." },
                        max: { value: 10000000000, message: "Maximum amount is ten billion (10,000,000,000)." },
                    }} />
                    <FormSelect control={control} name="sourceCurrencyId" label="Source Currency" options={getCurrencyOptions(currencies)} rules={{
                        validate: {
                            required: (value) => value && value !== getEmptyGuid() ? true : 'Source Currency is required.'
                        }
                    }} />

                    <FormNumber control={control} name="targetAmount" label="Target Amount" formatted precision={2} rules={{
                        required: 'Target Amount is required.',
                        min: { value: 0, message: "Minimum amount is 0." },
                        max: { value: 10000000000, message: "Maximum amount is ten billion (10,000,000,000)." },
                    }} />
                    <FormSelect control={control} name="targetCurrencyId" label="Target Currency" options={getCurrencyOptions(currencies)} rules={{
                        validate: {
                            required: (value) => value && value !== getEmptyGuid() ? true : 'Target Currency is required.'
                        }
                    }} />

                    <FormNumber control={control} name="exchangeRate" label="Rate (%)" formatted precision={2} rules={{
                        required: 'Rate is required.',
                        min: { value: 0, message: "Minimum exchange rate is 0." },
                        max: { value: 100, message: "Maximum exchange rate is 100." },
                    }} />

                    <FormTextArea control={control} name="description" label="Description" rules={{
                        maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                    }} />
                    {transactionId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
                </DrawerSection>
            </Form>
        </Drawer>
    );
};

export default CurrencyExchangeDetail;