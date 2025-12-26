import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getCurrencyById } from '../apis';

const useGetCurrencyById = () => {
    const [currencyId, setCurrencyId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCurrencyById-${currencyId}`],
        queryFn: async () => {
            if (!currencyId) return null;

            const apiResponse = await getCurrencyById(currencyId);
            return apiResponse.value;
        },
        enabled: Boolean(currencyId)
    });

    return {entity: data, isEntityLoading: isLoading, setCurrencyId   : setCurrencyId};
};

export default useGetCurrencyById;