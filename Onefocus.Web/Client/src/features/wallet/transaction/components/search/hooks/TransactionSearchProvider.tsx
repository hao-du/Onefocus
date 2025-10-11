import { PropsWithChildren, useCallback } from 'react';
import PeerTransferContext from './TransactionSearchContext';
import TransactionSearchFormInput from '../interfaces/TransactionSearchFormInput';
import useSearchTransactions from '../../../services/search/useSearchTransactions';

type TransactionSearchProviderProps = PropsWithChildren;

const TransactionSearchProvider = (props: TransactionSearchProviderProps) => {
    const { onSearchAsync, isSearching } = useSearchTransactions();

    const onSearch = useCallback(async (data: TransactionSearchFormInput) => {
        const response = await onSearchAsync({
            text: data.text
        });
        return response;
    }, [onSearchAsync]);

    // Provide the cash flow context to the children components
    return (
        <PeerTransferContext.Provider value={{
            onSearch: onSearch,
            isSearching: isSearching,
        }}>
            {props.children}
        </PeerTransferContext.Provider>
    );
};

export default TransactionSearchProvider;