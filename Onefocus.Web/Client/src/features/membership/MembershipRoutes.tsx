import { Route, Routes } from 'react-router';
import { NotFound } from '../../shared/app';
import { User } from './user';

const MembershipRoutes = () => {
    return (
        <Routes>
            <Route path="/user" element={<User />} />
            <Route path="/*" element={<NotFound />} />
        </Routes>
    );
}

export default MembershipRoutes;