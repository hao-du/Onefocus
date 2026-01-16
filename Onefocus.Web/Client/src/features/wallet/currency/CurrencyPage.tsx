import PageProvider from "../../../shared/hooks/page/PageProvider";
import CurrencyDetail from "./CurrencyDetail";
import CurrencyList from "./CurrencyList";

const CurrencyPage = () => {
    return (
        <PageProvider>
            <CurrencyList />
            <CurrencyDetail />
        </PageProvider>
    );
}

export default CurrencyPage;
