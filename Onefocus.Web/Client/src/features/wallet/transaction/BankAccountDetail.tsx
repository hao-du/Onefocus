import { useEffect, useMemo } from "react";
import { useForm, useWatch } from "react-hook-form";
import dayjs, { Dayjs } from "dayjs";
import useGetAllCurrencies from "../currency/services/useGetAllCurrencies";
import useCreateBankAccount from "./services/bank-account/useCreateBankAccount";
import useGetBankAccountByTransactionId from "./services/bank-account/useGetBankAccountByTransactionId";
import useUpdateBankAccount from "./services/bank-account/useUpdateBankAccount";
import { getEmptyGuid } from "../../../shared/utils";
import Drawer from "../../../shared/components/molecules/panels/Drawer";
import Form from "../../../shared/components/molecules/forms/Form";
import DrawerSection from "../../../shared/components/molecules/panels/DrawerSection";
import FormDatePicker from "../../../shared/components/molecules/forms/FormDatePicker";
import FormNumber from "../../../shared/components/molecules/forms/FormNumber";
import FormSelect from "../../../shared/components/molecules/forms/FormSelect";
import FormTextArea from "../../../shared/components/molecules/forms/FormTextArea";
import FormSwitch from "../../../shared/components/molecules/forms/FormSwitch";
import FormRepeater from "../../../shared/components/molecules/forms/FormRepeater";
import FormText from "../../../shared/components/molecules/forms/FormText";
import { TRANSACTION_COMPONENT_NAMES } from "../../constants";
import Icon from "../../../shared/components/atoms/misc/Icon";
import usePage from "../../../shared/hooks/page/usePage";
import useWindows from "../../../shared/hooks/windows/useWindows";
import useGetBanks from "../bank/services/useGetBanks";
import { getBankOptions, getCurrencyOptions } from "./shared";



interface BankAccountTransactionInput {
    id?: string;
    transactedOn: Dayjs;
    amount: number;
    description?: string;
    isActive: boolean;
}

interface BankAccountDetailInput {
    id?: string;
    amount: number;
    currencyId: string;
    interestRate: number;
    accountNumber: string;
    issuedOn: Dayjs;
    closedOn?: Dayjs;
    isClosed: boolean;
    bankId: string;
    description?: string;
    isActive: boolean;
    transactions: BankAccountTransactionInput[];
}

const BankAccountDetail = () => {
    const { isActiveComponent, closeComponent, dataId, setDataId, setLoadings, hasAnyLoading, expandDrawerTrigger: triggerSignal } = usePage();
    const { showResponseToast } = useWindows();

    const { bankAccount, isBankAccountLoading } = useGetBankAccountByTransactionId(dataId);
    const { currencies, isCurrenciesLoading } = useGetAllCurrencies();
    const { banks, isBanksLoading } = useGetBanks({})
    const { createBankAccountAsync, isBankAccountCreating } = useCreateBankAccount();
    const { updateBankAccountAsync, isBankAccountUpdating } = useUpdateBankAccount();

    const formValues = useMemo<BankAccountDetailInput>(() => {
        return dataId && bankAccount ? {
            ...bankAccount,
            issuedOn: dayjs(bankAccount.issuedOn),
            closedOn: dayjs(bankAccount.closedOn),
            transactions: !bankAccount.transactions ? [] : bankAccount.transactions.map((transaction) => ({ ...transaction, transactedOn: dayjs(transaction.transactedOn) }))
        } : {
            id: undefined,
            amount: 0,
            currencyId: getEmptyGuid(),
            interestRate: 1,
            accountNumber: '',
            issuedOn: dayjs(new Date()),
            closedOn: undefined,
            isClosed: false,
            bankId: getEmptyGuid(),
            isActive: true,
            description: '',
            transactions: []
        }
    }, [dataId, bankAccount]);

    const form = useForm<BankAccountDetailInput>({
        defaultValues: formValues,
        values: formValues
    });
    const { control, handleSubmit } = form;

    const isClosed = useWatch({
        control,
        name: "isClosed",
        defaultValue: false,
    });

    useEffect(() => {
        setLoadings({ isBankAccountLoading, isBankAccountCreating, isBankAccountUpdating, isCurrenciesLoading, isBanksLoading });
    }, [setLoadings, isCurrenciesLoading, isBankAccountLoading, isBankAccountCreating, isBankAccountUpdating, isBanksLoading]);

    const onSave = handleSubmit(async (data: BankAccountDetailInput) => {
        if (!data.id) {
            const response = await createBankAccountAsync({
                amount: data.amount,
                currencyId: data.currencyId,
                interestRate: data.interestRate,
                accountNumber: data.accountNumber,
                issuedOn: data.issuedOn.toDate(),
                closedOn: data.closedOn?.toDate(),
                isClosed: data.isClosed,
                bankId: data.bankId,
                description: data.description,
                transactions: data.transactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn.toDate(),
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
            const response = await updateBankAccountAsync({
                id: data.id,
                amount: data.amount,
                currencyId: data.currencyId,
                interestRate: data.interestRate,
                accountNumber: data.accountNumber,
                issuedOn: data.issuedOn.toDate(),
                closedOn: data.closedOn?.toDate(),
                isClosed: data.isClosed,
                bankId: data.bankId,
                description: data.description,
                isActive: data.isActive,
                transactions: data.transactions.map(item => ({
                    id: item.id,
                    transactedOn: item.transactedOn.toDate(),
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
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
            title={`${!dataId ? 'Create' : 'Edit'} Bank Account`}
            open={isActiveComponent(TRANSACTION_COMPONENT_NAMES.BankAccount)}
            onClose={closeComponent}
            showPrimaryButton
            expandDrawerTrigger={triggerSignal}
            actions={[
                {
                    id: 'btnSaveBankAccount',
                    label: 'Save',
                    command: onSave,
                    icon: <Icon name="save" />,
                    isPending: hasAnyLoading
                }
            ]}
        >
            <Form>
                <DrawerSection paddingTop>
                    <FormDatePicker focus control={control} name="issuedOn" label="Issued On" rules={{
                        required: 'Issued On is required.',
                    }} />
                    <FormSelect control={control} name="bankId" label="Bank" options={getBankOptions(banks)} rules={{
                        validate: (value) => value && value !== getEmptyGuid() ? true : 'Bank is required.'
                    }} />
                    <FormText control={control} name="accountNumber" label="Account Number" rules={{
                        maxLength: { value: 50, message: 'Account Number cannot exceed 50 characters.' },
                    }} />
                    <FormNumber control={control} name="interestRate" label="Interest Rate (%)" formatted precision={2} rules={{
                        required: 'Interest Rate is required.',
                        min: { value: 0.01, message: "Minimum interest rate is 0.01." },
                        max: { value: 100, message: "Maximum interest rate is 100." },
                    }} />
                    <FormNumber control={control} name="amount" label="Amount" formatted precision={2} rules={{
                        required: 'Amount is required.',
                        min: { value: 0, message: "Minimum amount is 0." },
                        max: { value: 10000000000, message: "Maximum amount is ten billion (10,000,000,000)." },
                    }} />
                    <FormSwitch control={control} name="isClosed" checkedLabel="Closed" uncheckedLabel="Open" extra="Toggle between open and closed account." />
                    {
                        isClosed &&
                        <FormDatePicker control={control} name="closedOn" label="Closed On" rules={{
                            required: 'Closed On is required when account is closed.',
                        }} />
                    }
                    <FormSelect control={control} name="currencyId" label="Currency" options={getCurrencyOptions(currencies)} rules={{
                        validate: (value) => value && value !== getEmptyGuid() ? true : 'Currency is required.',
                    }} />
                    <FormTextArea control={control} name="description" label="Description" rules={{
                        maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' },
                    }} />
                    {dataId && <FormSwitch control={control} name="isActive" checkedLabel="Active" uncheckedLabel="Inactive" />}
                </DrawerSection>
                <FormRepeater
                    title="Interests"
                    form={form}
                    path='transactions'
                    defaultRowValue={{
                        transactedOn: dayjs(new Date()),
                        amount: 0,
                        description: '',
                        isActive: true,
                    }}
                    render={(_, control, index, isReadMode, isFocused) => {
                        return (
                            <>
                                <FormDatePicker focus={isFocused} readOnly={isReadMode} control={control} name={`transactions.${index}.transactedOn`} label="Transacted On" rules={{
                                    required: 'Transacted On is required.'
                                }} />
                                <FormNumber readOnly={isReadMode} control={control} label="Amount" formatted precision={2} name={`transactions.${index}.amount`} rules={{
                                    required: 'Amount is required.',
                                    min: { value: 0, message: "Minimum amount is 0." },
                                    max: { value: 10000000000, message: "Maximum amount is ten billion." },
                                }} />
                                <FormTextArea readOnly={isReadMode} control={control} label="Description" rows={2} name={`transactions.${index}.description`} rules={{
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

export default BankAccountDetail;