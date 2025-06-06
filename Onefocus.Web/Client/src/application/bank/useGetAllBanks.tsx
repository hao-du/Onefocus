import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getAllBanks, getAllBanksAdapter} from '../../infrastructure/modules/bank';

export const useGetAllBanks = () => {
    const {client} = useClient();

    const {data,  isLoading, refetch, isFetching} = useQuery({
        queryKey: ['getAllBanks'],
        queryFn: async () => {
            const apiResponse = await getAllBanks(client);
            return getAllBanksAdapter().toBankEntities(apiResponse.value);
        }
    });

    return {banks: data, isListLoading: isLoading || isFetching, refetch};
};