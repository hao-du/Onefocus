import { CashFlow } from "../../../../../domain/transactions/cashFlow";
import { CashFlowFormInput } from "../../cashflow/interfaces/CashFlowFormInput";

export type CashFlowContextValue = {
    selectedCashFlow:  CashFlow | null | undefined;
    isCashFlowLoading: boolean;
    setTransactionIdFromCashFlow: (value: string | null) => void;
    onCashFlowSubmit: (data: CashFlowFormInput) => void;
};