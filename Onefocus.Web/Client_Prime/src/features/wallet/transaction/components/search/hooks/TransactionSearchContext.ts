import { createContext } from 'react';
import TransactionSearchContextValue from './TransactionSearchContextValue';

const TransactionSearchContext = createContext<TransactionSearchContextValue | null>(null);

export default TransactionSearchContext;