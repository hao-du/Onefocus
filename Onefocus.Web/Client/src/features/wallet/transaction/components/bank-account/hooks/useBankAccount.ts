import { useContext } from "react";
import BankAccountContext from "./BankAccountContext";

const useBankAccount = () => {
    const context = useContext(BankAccountContext);
    if (!context) {
        throw new Error('useBankAccount must be used within the BankAccountProvider');
    }
    return context;
};

export default useBankAccount;