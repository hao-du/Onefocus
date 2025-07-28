import { useQuery } from '@tanstack/react-query';
import { getAllTransactions } from '../apis';

const useGetAllTransactions = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['getAllTransactions'],
        queryFn: async () => {
            const apiResponse = await getAllTransactions();
            return apiResponse.value.transactions;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllTransactions;