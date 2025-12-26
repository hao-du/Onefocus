import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getAllLocaleOptions } from '../apis';

const useGetAllLocales = () => {
    const [isFetched, setIsFetched] = useState<boolean>(false);

    const {data, isLoading, isFetching} = useQuery({
        queryKey: [`getAllLocaleOptions`],
        queryFn: async () => {
            const apiResponse = await getAllLocaleOptions();
            setIsFetched(true);
            return apiResponse.value?.locales;
        },
        staleTime: isFetched ? Infinity: 0
    });

    return {entity: data, isEntityLoading: isLoading || isFetching};
};

export default useGetAllLocales;