import { PropsWithChildren } from 'react';
import { useGetAllCurrencies } from '../../../../currency/services';
import { useGetAllTransactions } from '../../../services';
import TransactionListContext from './TransactionListContext';
import { useGetAllBanks } from '../../../../bank/services';
import { useGetAllCounterparties } from '../../../../counterparty/services';

type TransactionListProviderProps = PropsWithChildren;

const TransactionListProvider = (props: TransactionListProviderProps) => {
    const { entities: currencies, isListLoading: isCurrenciesLoading } = useGetAllCurrencies();
    const { entities: banks, isListLoading: isBanksLoading } = useGetAllBanks();
    const { entities: transactions, isListLoading, refetch } = useGetAllTransactions();
    const { entities: counterparties, isListLoading: isCounterpartiesLoading } = useGetAllCounterparties();

    return (
        <TransactionListContext.Provider value={{
            transactions: transactions ?? [],
            currencies: currencies ?? [],
            banks: banks ?? [],
            counterparties: counterparties ?? [],
            isListLoading: isBanksLoading || isCurrenciesLoading || isCounterpartiesLoading || isListLoading,
            refetchList: refetch
        }}>
            {props.children}
        </TransactionListContext.Provider>
    );
};

export default TransactionListProvider;