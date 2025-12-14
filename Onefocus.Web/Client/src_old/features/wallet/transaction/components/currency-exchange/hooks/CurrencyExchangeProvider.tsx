import { PropsWithChildren, useCallback } from 'react';
import { useWindows } from '../../../../../../shared/components/hooks';
import { useCreateCurrencyExchange, useGetCurrencyExchangeByTransactionId, useUpdateCurrencyExchange } from '../../../services';
import { useTransactionList } from '../../transaction-list';
import CurrencyExchangeFormInput from '../interfaces/CurrencyExchangeFormInput';
import CurrencyExchangeContext from './CurrencyExchangeContext';
import { useTransactionPage } from '../../../pages/hooks';

type CurrencyExchangeProviderProps = PropsWithChildren;

const CurrencyExchangeProvider = (props: CurrencyExchangeProviderProps) => {
    const { setShowForm } = useTransactionPage();
    const { refetchList } = useTransactionList();
    const { showResponseToast } = useWindows();

    const { currencyExchangeEntity: selectedCurrencyExchange, isCurrencyExchangeLoading, setCurrencyExchangeTransactionId } = useGetCurrencyExchangeByTransactionId();
    const { onCreateAsync, isCreating } = useCreateCurrencyExchange();
    const { onUpdateAsync, isUpdating } = useUpdateCurrencyExchange();

    const onCurrencyExchangeSubmit = useCallback(async (data: CurrencyExchangeFormInput) => {
        if (!data.id) {
            const response = await onCreateAsync({
                sourceAmount: data.sourceAmount,
                sourceCurrencyId: data.sourceCurrencyId,
                targetAmount: data.targetAmount,
                targetCurrencyId: data.targetCurrencyId,
                exchangeRate: data.exchangeRate,
                transactedOn: data.transactedOn,
                description: data.description,
            });
            showResponseToast(response, 'Saved successfully.');
            if (response.status === 200 && response.value.id) {
                setCurrencyExchangeTransactionId(response.value.id);
            }
        } else {
            const response = await onUpdateAsync({
                id: data.id,
                sourceAmount: data.sourceAmount,
                sourceCurrencyId: data.sourceCurrencyId,
                targetAmount: data.targetAmount,
                targetCurrencyId: data.targetCurrencyId,
                exchangeRate: data.exchangeRate,
                transactedOn: data.transactedOn,
                description: data.description,
                isActive: data.isActive,
            });
            showResponseToast(response, 'Updated successfully.');
            if (!data.isActive) {
                setShowForm(false);
            }
        }
        refetchList();
    }, [onCreateAsync, onUpdateAsync, setShowForm, refetchList, setCurrencyExchangeTransactionId, showResponseToast]);

    // Provide the cash flow context to the children components
    return (
        <CurrencyExchangeContext.Provider value={{
            selectedCurrencyExchange: selectedCurrencyExchange,
            isCurrencyExchangeLoading: isCurrencyExchangeLoading || isCreating || isUpdating,
            setCurrencyExchangeTransactionId: setCurrencyExchangeTransactionId,
            onCurrencyExchangeSubmit: onCurrencyExchangeSubmit,
        }}>
            {props.children}
        </CurrencyExchangeContext.Provider>
    );
};

export default CurrencyExchangeProvider;