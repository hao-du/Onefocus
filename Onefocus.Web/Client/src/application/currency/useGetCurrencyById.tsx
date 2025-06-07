import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {
    getCurrencyById,
    getCurrencyByIdAdapter
} from '../../infrastructure/modules/currency';
import {useState} from 'react';

export const useGetCurrencyById = () => {
    const {client} = useClient();
    const [currencyId, setCurrencyId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCurrencyById-${currencyId}`],
        queryFn: async () => {
            if (!currencyId) return null;

            const apiResponse = await getCurrencyById(client, currencyId);
            return getCurrencyByIdAdapter().toCurrencyEntity(apiResponse.value);
        },
        enabled: Boolean(currencyId)
    });

    return {entity: data, isEntityLoading: isLoading, setCurrencyId   : setCurrencyId};
};