import useGetUser from "../../../application/user/useGetUser";

const Dashboard = () => {
    const { data } = useGetUser();

    return (
        <>
            <div>Dashboard</div>
            <ul>{data && data.map((user) => <li key={user.id}>{user.email} - {user.userName}</li>)}</ul>
        </>
    );
};

export default Dashboard;