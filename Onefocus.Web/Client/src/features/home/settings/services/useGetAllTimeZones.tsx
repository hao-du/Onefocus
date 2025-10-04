import { useQuery } from '../../../../shared/hooks';
import { getAllTimeZoneOptions } from '../apis';

const useGetAllTimeZones = () => {
    const {data, isLoading, isFetching} = useQuery({
        queryKey: [`getAllTimeZoneOptions`],
        queryFn: async () => {
            const apiResponse = await getAllTimeZoneOptions();
            return apiResponse.value?.timeZones;
        },
        staleTime: Infinity
    });

    return {entity: data, isEntityLoading: isLoading || isFetching};
};

export default useGetAllTimeZones;