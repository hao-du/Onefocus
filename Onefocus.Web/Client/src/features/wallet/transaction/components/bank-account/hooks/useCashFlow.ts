import { useContext } from "react";
import BankAccountContext from "./BankAccountContext";

const useCashFlow = () => {
    const context = useContext(BankAccountContext);
    if (!context) {
        throw new Error('useCashFlow must be used within the CashFlowProvider');
    }
    return context;
};

export default useCashFlow;