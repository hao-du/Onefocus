import PageProvider from "../../../shared/hooks/page/PageProvider";
import HomeDetail from "./HomeDetail";

const HomePage = () => {
    return (
        <PageProvider>
            <HomeDetail />
        </PageProvider>
    );
}

export default HomePage;
