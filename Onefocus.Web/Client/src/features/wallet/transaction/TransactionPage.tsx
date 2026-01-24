import PageProvider from "../../../shared/hooks/page/PageProvider";
import BankAccountDetail from "./BankAccountDetail";
import CashFlowDetail from "./CashFlowDetail";
import TransactionList from "./TransactionList";

const TransactionPage = () => {
    return (
        <PageProvider>
            <TransactionList />
            <CashFlowDetail />
            <BankAccountDetail />
        </PageProvider>
    );
}

export default TransactionPage;
