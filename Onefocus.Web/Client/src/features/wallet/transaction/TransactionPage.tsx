import PageProvider from "../../../shared/hooks/page/PageProvider";
import CashFlowDetail from "./CashFlowDetail";
import TransactionList from "./TransactionList";

const TransactionPage = () => {
    return (
        <PageProvider>
            <TransactionList />
            <CashFlowDetail />
        </PageProvider>
    );
}

export default TransactionPage;
