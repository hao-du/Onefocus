import { useContext } from 'react';
import TransactionListContext from './TransactionListContext';

const useTransactionList = () => {
    const context = useContext(TransactionListContext);
    if (!context) {
        throw new Error('useTransactionList must be used within the TransactionListProvider');
    }
    return context;
};

export default useTransactionList;