import { createContext } from 'react';
import BankAccountContextValue from './BankAccountContextValue';

const BankAccountContext = createContext<BankAccountContextValue | null>(null);

export default BankAccountContext;