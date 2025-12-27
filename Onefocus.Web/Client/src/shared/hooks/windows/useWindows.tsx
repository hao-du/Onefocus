import {useContext} from 'react';
import WindowsContext from './WindowsContext';

const useWindows = () => {
    const context = useContext(WindowsContext);
    if (!context) {
        throw new Error('useWindows must be used within the WindowsProvider');
    }
    return context;
}

export default useWindows;