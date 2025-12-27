import { useContext } from 'react';
import LocaleContext from './LocaleContext';

const useLocale = () => {
    const context = useContext(LocaleContext);
    if (!context) {
        throw new Error('useLocale must be used within the LocaleProvider');
    }
    return context;
};

export default useLocale;