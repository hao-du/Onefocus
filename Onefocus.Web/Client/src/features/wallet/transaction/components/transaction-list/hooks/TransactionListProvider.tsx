import { PropsWithChildren } from 'react';
import { useGetAllCurrencies } from '../../../../currency/services';
import { useGetAllTransactions } from '../../../services';
import TransactionListContext from './TransactionListContext';

type TransactionListProviderProps = PropsWithChildren;

const TransactionListProvider = (props: TransactionListProviderProps) => {
    const { entities: currencies, isListLoading: isCurrenciesLoading } = useGetAllCurrencies();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();

    return (
        <TransactionListContext.Provider value={{
            transactions: transactions ?? [],
            currencies: currencies ?? [],
            isListLoading: isCurrenciesLoading || isListLoading,
            refetchList: refetch
        }}>
            {props.children}
        </TransactionListContext.Provider>
    );
};

export default TransactionListProvider;