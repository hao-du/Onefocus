import { ReactNode, useCallback } from 'react';
import { CashFlowFormInput } from '../../cashflow/CashFlowFormInput';
import { useTransaction } from '../transaction/useTransaction';
import { useCreateCashFlow, useGetCashFlowByTransactionId } from '../../../../../../application/transaction';
import { useWindows } from '../../../../../components/hooks/useWindows';
import { CashFlowContext } from './CashFlowContext';

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
                transactionItems: data.transactionItems.map(item => ({
                    id: item.id,
                    name: item.name,
                    amount: item.amount,
                    isActive: item.isActive,
                    description: item.description
                }))
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setTransactionIdFromCashFlow(response.value.id);
            }
        }
        refetchList();
    }, [onCreateAsync, refetchList, setTransactionIdFromCashFlow, showResponseToast]);

    // Provide the authentication context to the children components
    return (
        <CashFlowContext.Provider value={{
            selectedCashFlow: selectedCashFlow,
            isCashFlowLoading: isCashFlowLoading || isCreating,
            setTransactionIdFromCashFlow: setTransactionIdFromCashFlow,
            onCashFlowSubmit: onCashFlowSubmit
        }}>
            {children}
        </CashFlowContext.Provider>
    );
};