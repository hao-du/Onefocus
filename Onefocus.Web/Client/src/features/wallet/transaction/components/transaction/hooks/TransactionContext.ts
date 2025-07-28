import { createContext } from 'react';
import TransactionContextValue from './TransactionContextValue';

const TransactionContext = createContext<TransactionContextValue | null>(null);

export default TransactionContext;