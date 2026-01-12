import BankFilterForm from "./BankFilter";
import PageProvider from "../../../shared/hooks/page/PageProvider";
import BankList from "./BankList";
import BankDetail from "./BankDetail";
import GetBanksRequest from "../apis/interfaces/GetBanksRequest";

const BankPage = () => {
    return (
        <PageProvider<GetBanksRequest>>
            <BankList />
            <BankDetail />
            <BankFilterForm />
        </PageProvider>
    );
}

export default BankPage;
