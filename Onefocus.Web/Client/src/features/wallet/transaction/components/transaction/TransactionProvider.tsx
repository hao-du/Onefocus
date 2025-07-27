import { ReactNode } from 'react';
import {useGetAllCurrencies} from '../../../currency/services';
import {useGetAllTransactions} from '../../services/useGetAllTransactions';
import { TransactionContext } from './TransactionContext';

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