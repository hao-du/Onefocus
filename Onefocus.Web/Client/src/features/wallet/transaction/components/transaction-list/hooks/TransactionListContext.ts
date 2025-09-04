import { createContext } from 'react';
import TransactionListContextValue from './TransactionListContextValue';

const TransactionListContext = createContext<TransactionListContextValue | null>(null);

export default TransactionListContext;