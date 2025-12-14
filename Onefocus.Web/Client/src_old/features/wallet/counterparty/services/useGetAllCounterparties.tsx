
import { useQuery } from '../../../../shared/hooks';
import { getAllCounterparties } from '../apis';

const useGetAllCounterparties = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['getAllCounterparties'],
        queryFn: async () => {
            const apiResponse = await getAllCounterparties();
            return apiResponse.value.counterparties;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllCounterparties;