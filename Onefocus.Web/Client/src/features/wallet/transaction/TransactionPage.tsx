import PageProvider from "../../../shared/hooks/page/PageProvider";
import BankAccountDetail from "./BankAccountDetail";
import CashFlowDetail from "./CashFlowDetail";
import CurrencyExchangeDetail from "./CurrencyExchangeDetail";
import PeerTransferDetail from "./PeerTransferDetail";
import TransactionList from "./TransactionList";

const TransactionPage = () => {
    return (
        <PageProvider>
            <TransactionList />
            <BankAccountDetail />
            <CashFlowDetail />
            <CurrencyExchangeDetail />
            <PeerTransferDetail />
        </PageProvider>
    );
}

export default TransactionPage;
