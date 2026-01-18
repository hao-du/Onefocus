
import { useQuery } from '@tanstack/react-query';
import transactionApi from '../../apis/transactionApi';

const useGetAllTransactions = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['transaction', 'useGetTransactions'],
        queryFn: async () => {
            const apiResponse = await transactionApi.getAllTransactions();
            return apiResponse.value.transactions;
        }
    });

    return { transactions: data, isTransactionsLoading: isLoading || isFetching, refetch };
};

export default useGetAllTransactions;