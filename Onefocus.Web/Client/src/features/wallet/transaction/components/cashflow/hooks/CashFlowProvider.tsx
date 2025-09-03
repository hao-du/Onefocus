import { PropsWithChildren, useCallback } from 'react';
import { useWindows } from '../../../../../../shared/components/hooks';
import { useCreateCashFlow, useGetCashFlowByTransactionId, useUpdateCashFlow } from '../../../services';
import { useTransaction } from '../../transaction';
import CashFlowFormInput from '../interfaces/CashFlowFormInput';
import CashFlowContext from './CashFlowContext';

type CashFlowProviderProps = PropsWithChildren;

const CashFlowProvider = (props: CashFlowProviderProps) => {
    const { refetchList, setShowForm } = useTransaction();
    const { showResponseToast } = useWindows();

    const { cashFlowEntity: selectedCashFlow, isCashFlowLoading, setCashFlowTransactionId} = useGetCashFlowByTransactionId();
    const { onCreateAsync, isCreating } = useCreateCashFlow();
    const { onUpdateAsync, isUpdating } = useUpdateCashFlow();

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
                setCashFlowTransactionId(response.value.id);
            }
        } else {
            const response = await onUpdateAsync({
                id: data.id,
                amount: data.amount,
                currencyId: data.currencyId,
                isIncome: data.isIncome,
                isActive: data.isActive,
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
            showResponseToast(response, 'Updated successfully.');
            if(!data.isActive) {
               setShowForm(false);
            }
        }
        refetchList();
    }, [onCreateAsync, onUpdateAsync, setShowForm, refetchList, setCashFlowTransactionId, showResponseToast]);

    // Provide the cash flow context to the children components
    return (
        <CashFlowContext.Provider value={{
            selectedCashFlow: selectedCashFlow,
            isCashFlowLoading: isCashFlowLoading || isCreating || isUpdating,
            setCashFlowTransactionId: setCashFlowTransactionId,
            onCashFlowSubmit: onCashFlowSubmit,
        }}>
            {props.children}
        </CashFlowContext.Provider>
    );
};

export default CashFlowProvider;