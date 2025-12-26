import BankAccountFormInput from "../interfaces/BankAccountFormInput";

type BankAccountContextValue = {
    selectedBankAccount:  BankAccountFormInput | null | undefined;
    isBankAccountLoading: boolean;
    setBankAccountTransactionId: (value: string | null) => void;
    onBankAccountSubmit: (data: BankAccountFormInput) => void;
};

export default BankAccountContextValue;