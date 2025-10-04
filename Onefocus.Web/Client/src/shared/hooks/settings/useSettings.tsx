import { useContext } from 'react';
import SettingsContext from './SettingsContext';

const useSettings = () => {
    const context = useContext(SettingsContext);
    if (!context) {
        throw new Error('useSettings must be used within the SettingsProvider');
    }
    return context;
};

export default useSettings;