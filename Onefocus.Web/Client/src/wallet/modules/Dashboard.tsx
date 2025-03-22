import {useQuery} from "@tanstack/react-query";
import useUserApi from "../../api/users/useUserApi.tsx";

const Dashboard = () => {
    const {getAllUsers} = useUserApi();

    const {data} = useQuery({
        queryKey: ['Dashboard'],
        queryFn: async () => {
            return await getAllUsers();
        },
        retry: 2
    });

    return (
        <>
            <div>Dashboard</div>
            <ul>{data && data.value.users.map((user) => <li key={user.id}>{user.email} - {user.userName}</li>)}</ul>
        </>
    );
};

export default Dashboard;