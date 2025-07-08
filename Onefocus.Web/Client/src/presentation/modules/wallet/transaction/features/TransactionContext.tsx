import {createContext, ReactNode, useContext} from 'react';
import {useGetAllCurrencies} from '../../../../../application/currency';
import {useGetAllTransactions} from '../../../../../application/transaction/useGetAllTransactions';
import {Currency} from '../../../../../domain/currency';
import {RefetchOptions} from '@tanstack/react-query';
import {Transaction} from '../../../../../domain/transaction';

export type TransactionContextValue = {
    transactions:  Transaction[];
    currencies: Currency[];
    isListLoading: boolean;
    refetchList: (options?: RefetchOptions) => void
};

const TransactionContext = createContext<TransactionContextValue>({
    transactions: [],
    currencies: [],
    isListLoading: false,
    refetchList: () => {}
});

export const useTransaction = () => {
    const context = useContext(TransactionContext);
    if (!context) {
        throw new Error('useTransaction must be used within the TransactionProvider');
    }
    return context;
};

export const TransactionProvider = ({ children }: { children: ReactNode }) => {
    const { entities: currencies, isListLoading: isCurrenciesLoading} = useGetAllCurrencies();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();

    return (
        <TransactionContext.Provider value={{
            transactions: transactions ?? [],
            currencies: currencies ?? [],
            isListLoading: isCurrenciesLoading || isListLoading,
            refetchList: refetch
        }}>
            {children}
        </TransactionContext.Provider>
    );
};