import { useContext } from 'react';
import TransactionPageContext from './TransactionPageContext';

const useTransactionPage = () => {
    const context = useContext(TransactionPageContext);
    if (!context) {
        throw new Error('useTransactionPage must be used within the TransactionPageProvider');
    }
    return context;
};

export default useTransactionPage;