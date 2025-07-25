import { createContext } from 'react';
import { CashFlowContextValue } from './CashFlowContextValue';

export const CashFlowContext = createContext<CashFlowContextValue>({
    selectedCashFlow: null,
    isCashFlowLoading: false,
    setTransactionIdFromCashFlow: () => { },
    onCashFlowSubmit: () => { }
});