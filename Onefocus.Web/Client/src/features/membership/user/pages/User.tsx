import React from 'react';
import UserList from '../components/UserList';

const User = React.memo(() => {
    return <UserList />;
});

export default User;