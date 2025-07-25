import { useContext } from "react";
import { CashFlowContext } from "./CashFlowContext";

export const useCashFlow = () => {
    const context = useContext(CashFlowContext);
    if (!context) {
        throw new Error('useCashFlow must be used within the CashFlowProvider');
    }
    return context;
};