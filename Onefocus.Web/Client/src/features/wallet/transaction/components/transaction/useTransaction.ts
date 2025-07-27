import { useContext } from 'react';
import { TransactionContext } from './TransactionContext';

export const useTransaction = () => {
    const context = useContext(TransactionContext);
    if (!context) {
        throw new Error('useTransaction must be used within the TransactionProvider');
    }
    return context;
};