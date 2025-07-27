import {useContext} from 'react';
import ClientContext from './ClientContext';

const useClient = () => {
    const context = useContext(ClientContext);
    if (!context) {
        throw new Error('useClient must be used within the ClientProvider');
    }
    return context;
};

export default useClient;