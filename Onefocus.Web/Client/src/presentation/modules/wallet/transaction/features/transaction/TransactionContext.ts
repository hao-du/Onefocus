import { createContext } from 'react';
import { TransactionContextValue } from './TransactionContextValue';

export const TransactionContext = createContext<TransactionContextValue>({
    transactions: [],
    currencies: [],
    isListLoading: false,
    refetchList: () => { }
});

