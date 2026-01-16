import PageProvider from "../../../shared/hooks/page/PageProvider";
import CounterpartyDetail from "./CounterpartyDetail";
import CounterpartyList from "./CounterpartyList";

const CounterpartyPage = () => {
    return (
        <PageProvider>
            <CounterpartyList />
            <CounterpartyDetail />
        </PageProvider>
    );
}

export default CounterpartyPage;
