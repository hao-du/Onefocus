import {useContext} from 'react';
import TokenContext from './TokenContext';

const useToken = () => {
    const context = useContext(TokenContext);
    if (!context) {
        throw new Error('useToken must be used within the TokenProvider');
    }
    return context;
};

export default useToken;