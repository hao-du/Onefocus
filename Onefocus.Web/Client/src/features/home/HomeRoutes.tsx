import { Route, Routes } from 'react-router';
import { NotFound } from '../../shared/app';
import { Setting } from './setting';

const HomeRoutes = () => {
    return (
        <Routes>
            <Route path="/setting" element={<Setting />} />
            <Route path="/*" element={<NotFound />} />
        </Routes>
    );
}

export default HomeRoutes;