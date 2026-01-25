import dayjs, { Dayjs } from "dayjs";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import usePage from "../../../shared/hooks/page/usePage";
import useWindows from "../../../shared/hooks/windows/useWindows";
import useGetPeerTransferByTransactionId from "./services/peer-transfer/useGetPeerTransferByTransactionId";
import useGetAllCurrencies from "../currency/services/useGetAllCurrencies";
import useCreatePeerTransfer from "./services/peer-transfer/useCreatePeerTransfer";
import useUpdatePeerTransfer from "./services/peer-transfer/useUpdatePeerTransfer";
import { useForm } from "react-hook-form";
import { useEffect, useMemo } from "react";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import Form from "../../../shared/components/molecules/forms/Form";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";
import FormDatePicker from "../../../shared/components/molecules/forms/FormDatePicker";
import FormNumber from "../../../shared/components/molecules/forms/FormNumber";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";
import FormRepeater from "../../../shared/components/molecules/forms/FormRepeater";
import Icon from "../../../shared/components/atoms/misc/Icon";
import { getEmptyGuid } from "../../../shared/utils";
import useLocale from "../../../shared/hooks/locale/useLocale";
import useGetAllCounterparties from "../counterparty/services/useGetAllCounterparties";
import { getCounterpartyOptions, getCurrencyOptions } from "./shared";

interface PeerTransferTransactionInput {
    id?: string;
    transactedOn: Dayjs;
    amount: number;
    currencyId: string;
    description?: string;
    isInFlow?: boolean;
    isActive: boolean;
    rowId?: string
}

interface PeerTransferDetailInput {
    id?: string;
    status: number;
    type: number;
    counterpartyId: string;
    isActive: boolean;
    description?: string;
    transferTransactions: PeerTransferTransactionInput[];
}

const PeerTransferDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger } = usePage();

    const isActive = isActiveComponent(TRANSACTION_COMPONENT_NAMES.PeerTransfer);
    const transactionId = isActive ? dataId : undefined;

    const { showResponseToast } = useWindows();
    const { translate } = useLocale();

    const { peerTransfer, isPeerTransferLoading } = useGetPeerTransferByTransactionId(transactionId);
    const { currencies, isCurrenciesLoading } = useGetAllCurrencies();
    const { counterparties, isCounterpartiesLoading } = useGetAllCounterparties();
    const { createPeerTransferAsync, isPeerTransferCreating } = useCreatePeerTransfer();
    const { updatePeerTransferAsync, isPeerTransferUpdating } = useUpdatePeerTransfer();

    const formValues = useMemo<PeerTransferDetailInput>(() => {
        return transactionId && peerTransfer ? {
            ...peerTransfer,
            transferTransactions: !peerTransfer.transferTransactions ? [] : peerTransfer.transferTransactions.map((transaction) => ({ ...transaction, transactedOn: dayjs(transaction.transactedOn) }))
        } : {
            id: undefined,
            status: 0,
            type: 0,
            counterpartyId: getEmptyGuid(),
            description: undefined,
            isActive: true,
            transferTransactions: [],
        }
    }, [transactionId, peerTransfer]);

    const form = useForm<PeerTransferDetailInput>({
        defaultValues: formValues,
        values: formValues
    });
    const { control, handleSubmit } = form;

    useEffect(() => {
        setLoadings({ isPeerTransferLoading, isCurrenciesLoading, isPeerTransferCreating, isPeerTransferUpdating, isCounterpartiesLoading });
    }, [setLoadings, isCurrenciesLoading, isPeerTransferLoading, isPeerTransferCreating, isPeerTransferUpdating, isCounterpartiesLoading]);

    const onSave = handleSubmit(async (data: PeerTransferDetailInput) => {
        if (!data.id) {
            const response = await createPeerTransferAsync({
                status: data.status,
                type: data.type,
                counterpartyId: data.counterpartyId,
                description: data.description,
                transferTransactions: data.transferTransactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn.toDate(),
                    amount: item.amount,
                    currencyId: item.currencyId,
                    description: item.description,
                    isInFlow: item.isInFlow ?? false,
                    isActive: item.isActive,
                }))
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setDataId(response.value.id);
            }
        } else {
            const response = await updatePeerTransferAsync({
                id: data.id,
                status: data.status,
                type: data.type,
                counterpartyId: data.counterpartyId,
                description: data.description,
                isActive: data.isActive,
                transferTransactions: data.transferTransactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn.toDate(),
                    amount: item.amount,
                    currencyId: item.currencyId,
                    description: item.description,
                    isInFlow: item.isInFlow ?? false,
                    isActive: item.isActive,
                }))
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
                    <FormSelect control={control} name="type" label="Type" options={[
                        { label: translate('Lent to'), value: 100 },
                        { label: translate('Borrowed from'), value: 200 },
                        { label: translate('Gave to'), value: 300 },
                        { label: translate('Received from'), value: 400 }
                    ]} rules={{
                        validate: {
                            required: (value) => value && value != 0 ? true : 'Type is required.'
                        }
                    }} />

                    <FormSelect control={control} name="counterpartyId" label="Counterparty" options={getCounterpartyOptions(counterparties)} rules={{
                        validate: {
                            required: (value) => value && value !== getEmptyGuid() ? true : 'Counterparty is required.'
                        }
                    }} />

                    <FormSelect control={control} name="status" label="Status" options={[
                        { label: translate('Completed'), value: 100 },
                        { label: translate('Failed'), value: 200 },
                        { label: translate('Processing'), value: 300 },
                        { label: translate('Pending'), value: 400 }
                    ]} rules={{
                        validate: {
                            required: (value) => value && value != 0 ? true : 'Status is required.'
                        }
                    }} />

                    <FormTextArea control={control} name="description" label="Description" rules={{
                        maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                    }} />
                    {transactionId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
                </DrawerSection>
                <FormRepeater
                    title="Notes"
                    form={form}
                    path='transferTransactions'
                    defaultRowValue={{
                        transactedOn: dayjs(new Date()),
                        amount: 0,
                        currencyId: getEmptyGuid(),
                        description: '',
                        isInFlow: undefined,
                        isActive: true
                    }}
                    render={(_, control, index, isReadMode, isFocused) => {
                        return (
                            <>
                                <FormDatePicker focus={isFocused} showTime readOnly={isReadMode} control={control} name={`transferTransactions.${index}.transactedOn`} label="Transacted On" rules={{
                                    required: 'Transacted On is required.'
                                }} />
                                <FormNumber readOnly={isReadMode} control={control} label="Amount" formatted precision={2} name={`transferTransactions.${index}.amount`} rules={{
                                    required: 'Amount is required.',
                                    min: { value: 0, message: "Minimum amount is 0." },
                                    max: { value: 10000000000, message: "Maximum amount is ten billion." },
                                }} />
                                <FormSelect readOnly={isReadMode} control={control} name={`transferTransactions.${index}.currencyId`} label="Currency" options={getCurrencyOptions(currencies)} rules={{
                                    validate: {
                                        required: (value) => value && value !== getEmptyGuid() ? true : 'Currency is required.'
                                    }
                                }} />
                                <FormSwitch disabled={isReadMode} control={control} name={`transferTransactions.${index}.isInFlow`} label="Cash flow type" checkedLabel="Inflow" uncheckedLabel="Outflow" />

                                <FormTextArea readOnly={isReadMode} control={control} label="Description" rows={2} name={`transferTransactions.${index}.description`} rules={{
                                    maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                                }} />
                            </>
                        );
                    }}
                />
            </Form>
        </Drawer>
    );
};

export default PeerTransferDetail;