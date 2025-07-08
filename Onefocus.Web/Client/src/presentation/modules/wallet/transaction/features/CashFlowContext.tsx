import {createContext, ReactNode, useCallback, useContext} from 'react';
import {CashFlowFormInput} from '../cashflow/CashFlowFormInput';
import {CashFlow} from '../../../../../domain/transactions/cashFlow';
import {useTransaction} from './TransactionContext';
import {useCreateCashFlow, useGetCashFlowByTransactionId} from '../../../../../application/transaction';
import {useWindows} from '../../../../components/hooks/useWindows';

export type CashFlowContextValue = {
    selectedCashFlow:  CashFlow | null | undefined;
    isCashFlowLoading: boolean;
    setTransactionIdFromCashFlow: (value: string | null) => void;
    onCashFlowSubmit: (data: CashFlowFormInput) => void;
};

const CashFlowContext = createContext<CashFlowContextValue>({
    selectedCashFlow: null,
    isCashFlowLoading: false,
    setTransactionIdFromCashFlow: () => {},
    onCashFlowSubmit: () => {}
});

export const useCashFlow = () => {
    const context = useContext(CashFlowContext);
    if (!context) {
        throw new Error('useCashFlow must be used within the CashFlowProvider');
    }
    return context;
};

export const CashFlowProvider = ({ children }: { children: ReactNode }) => {
    const { refetchList } = useTransaction();
    const { showResponseToast } = useWindows();

    const { cashFlowEntity: selectedCashFlow, isCashFlowLoading, setTransactionId: setTransactionIdFromCashFlow } = useGetCashFlowByTransactionId();
    const { onCreateAsync, isCreating } = useCreateCashFlow()

    const onCashFlowSubmit = useCallback(async (data: CashFlowFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                amount: data.amount,
                currencyId: data.currencyId,
                isIncome: data.isIncome,
                transactedOn: data.transactedOn,
                description: data.description,
                transactionItems: []
            });
            showResponseToast(response, 'Saved successfully.');
            if(response.status === 200 && response.value.id) {
                setTransactionIdFromCashFlow(response.value.id);
            }
        }
        refetchList();
    }, []);

    // Provide the authentication context to the children components
    return (
        <CashFlowContext.Provider value={{
            selectedCashFlow: selectedCashFlow,
            isCashFlowLoading : isCashFlowLoading || isCreating,
            setTransactionIdFromCashFlow: setTransactionIdFromCashFlow,
            onCashFlowSubmit: onCashFlowSubmit
        }}>
            {children}
        </CashFlowContext.Provider>
    );
};