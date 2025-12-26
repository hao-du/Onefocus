import { createContext } from 'react';
import CashFlowContextValue from './CashFlowContextValue';

const CashFlowContext = createContext<CashFlowContextValue | null>(null);

export default CashFlowContext;