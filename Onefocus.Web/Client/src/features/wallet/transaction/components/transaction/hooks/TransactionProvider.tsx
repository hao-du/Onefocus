import { PropsWithChildren } from 'react';
import { useGetAllCurrencies } from '../../../../currency/services';
import { useGetAllTransactions } from '../../../services';
import TransactionContext from './TransactionContext';

type TransactionProviderProps = PropsWithChildren;

const TransactionProvider = (props: TransactionProviderProps) => {
    const { entities: currencies, isListLoading: isCurrenciesLoading} = useGetAllCurrencies();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();

    return (
        <TransactionContext.Provider value={{
            transactions: transactions ?? [],
            currencies: currencies ?? [],
            isListLoading: isCurrenciesLoading || isListLoading,
            refetchList: refetch
        }}>
            {props.children}
        </TransactionContext.Provider>
    );
};

export default TransactionProvider;