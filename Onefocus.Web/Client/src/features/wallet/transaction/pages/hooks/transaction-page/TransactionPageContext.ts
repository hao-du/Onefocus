import { createContext } from 'react';
import TransactionPageContextValue from './TransactionPageContextValue';

const TransactionPageContext = createContext<TransactionPageContextValue | null>(null);

export default TransactionPageContext;