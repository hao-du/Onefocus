import PageProvider from "../../../shared/hooks/page/PageProvider";
import NotFoundDetail from "./NotFoundDetail";

const NotFoundPage = () => {
    return (
        <PageProvider>
            <NotFoundDetail />
        </PageProvider>
    );
}

export default NotFoundPage;
