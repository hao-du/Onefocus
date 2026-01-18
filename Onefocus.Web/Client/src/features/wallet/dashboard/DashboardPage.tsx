import PageProvider from "../../../shared/hooks/page/PageProvider";
import DashboardDetail from "./DashboardDetail";

const DashboardPage = () => {
    return (
        <PageProvider>
            <DashboardDetail />
        </PageProvider>
    );
}

export default DashboardPage;
