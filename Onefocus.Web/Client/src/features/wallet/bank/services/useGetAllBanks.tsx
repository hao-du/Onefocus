
import { useQuery } from '../../../../shared/hooks';
import { getAllBanks } from '../apis';

const useGetAllBanks = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['getAllBanks'],
        queryFn: async () => {
            const apiResponse = await getAllBanks();
            return apiResponse.value.banks;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllBanks;