import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getAllTransactions, getAllTransactionsAdapter} from '../../infrastructure/modules/transaction';

export const useGetAllTransactions = () => {
    const {client} = useClient();

    const {data,  isLoading, refetch, isFetching} = useQuery({
        queryKey: ['getAllTransactions'],
        queryFn: async () => {
            const apiResponse = await getAllTransactions(client);
            return getAllTransactionsAdapter().toTransactionEntities(apiResponse.value);
        }
    });

    return {entities: data, isListLoading: isLoading || isFetching, refetch};
};