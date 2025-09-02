import { PropsWithChildren, useState } from 'react';
import { useGetAllCurrencies } from '../../../../currency/services';
import { useGetAllTransactions } from '../../../services';
import TransactionContext from './TransactionContext';

type TransactionProviderProps = PropsWithChildren;

const TransactionProvider = (props: TransactionProviderProps) => {
    const { entities: currencies, isListLoading: isCurrenciesLoading } = useGetAllCurrencies();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();
    const [ showForm, setShowForm] = useState(false);

    return (
        <TransactionContext.Provider value={{
            transactions: transactions ?? [],
            currencies: currencies ?? [],
            isListLoading: isCurrenciesLoading || isListLoading,
            refetchList: refetch,
            showForm: showForm,
            setShowForm: setShowForm
        }}>
            {props.children}
        </TransactionContext.Provider>
    );
};

export default TransactionProvider;