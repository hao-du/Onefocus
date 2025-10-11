import { useState } from 'react';
import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { SearchTransactionsRequest, SearchTransactionsResponse } from '../../apis/interfaces';
import { searchTransactions } from '../../apis/transactionApis';
import SearchCriteria from './interfaces/SearchCriteria';

const useSearchTransactions = () => {
    const [searchCriteria, setSearchCriteria] = useState<SearchCriteria>({});

    const {mutateAsync, isPending} = useMutation<ApiResponse<SearchTransactionsResponse>, unknown, SearchTransactionsRequest>({
        mutationFn: async (request) => {
            const response = searchTransactions(request);
            return response;
        }
    });

    return {onSearchAsync: mutateAsync, isSearching: isPending, searchCriteria, setSearchCriteria};
};

export default useSearchTransactions ;