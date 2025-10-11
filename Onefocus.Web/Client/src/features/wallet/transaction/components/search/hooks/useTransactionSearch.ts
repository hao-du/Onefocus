import { useContext } from "react";
import TransactionSearchContext from "./TransactionSearchContext";

const useTransactionSearch = () => {
    const context = useContext(TransactionSearchContext);
    if (!context) {
        throw new Error('usePeerTransfer must be used within the PeerTransferProvider');
    }
    return context;
};

export default useTransactionSearch;