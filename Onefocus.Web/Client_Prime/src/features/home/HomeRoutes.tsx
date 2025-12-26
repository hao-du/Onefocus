import { Route, Routes } from 'react-router';
import { NotFound } from '../../shared/app';
import { Settings } from './settings';

const HomeRoutes = () => {
    return (
        <Routes>
            <Route path="/settings" element={<Settings />} />
            <Route path="/*" element={<NotFound />} />
        </Routes>
    );
}

export default HomeRoutes;