import CashFlowFormInput from "../interfaces/CashFlowFormInput";

type CashFlowContextValue = {
    selectedCashFlow:  CashFlowFormInput | null | undefined;
    isCashFlowLoading: boolean;
    setCashFlowTransactionId: (value: string | null) => void;
    onCashFlowSubmit: (data: CashFlowFormInput) => void;
};

export default CashFlowContextValue;