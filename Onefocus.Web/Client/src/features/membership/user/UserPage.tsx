import PageProvider from "../../../shared/hooks/page/PageProvider";
import UpdatePassword from "./UpdatePassword";
import UserDetail from "./UserDetail";
import UserList from "./UserList";

const UserPage = () => {
    return (
        <PageProvider>
            <UserList />
            <UserDetail />
            <UpdatePassword />
        </PageProvider>
    );
}

export default UserPage;
