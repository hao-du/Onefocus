import { useQuery } from '../../../../shared/hooks';
import { getAllLocaleOptions } from '../apis';

const useGetAllLocales = () => {
    const {data, isLoading, isFetching} = useQuery({
        queryKey: [`getAllLocaleOptions`],
        queryFn: async () => {
            const apiResponse = await getAllLocaleOptions();
            return apiResponse.value?.locales;
        },
        staleTime: Infinity
    });

    return {entity: data, isEntityLoading: isLoading || isFetching};
};

export default useGetAllLocales;