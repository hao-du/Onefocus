import { useQuery } from '@tanstack/react-query';
import SearchCriteria from '../../../apis/interfaces/transaction/SearchCriteria';
import transactionApi from '../../../apis/transactionApi';

const useSearchTransactions = (searchCriteria: SearchCriteria) => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['transaction', 'useGetTransactions', searchCriteria],
        queryFn: async () => {
            const apiResponse = await transactionApi.searchTransactions(searchCriteria);
            return apiResponse.value.transactions;
        },
        enabled: Boolean(searchCriteria)
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useSearchTransactions;