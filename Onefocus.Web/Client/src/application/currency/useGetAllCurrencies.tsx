import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getAllCurrencies, getAllCurrenciesAdapter} from '../../infrastructure/modules/currency';

export const useGetAllCurrencies = () => {
    const {client} = useClient();

    const {data,  isLoading, refetch, isFetching} = useQuery({
        queryKey: ['getAllCurrencies'],
        queryFn: async () => {
            const apiResponse = await getAllCurrencies(client);
            return getAllCurrenciesAdapter().toCurrencyEntities(apiResponse.value);
        }
    });

    return {entities: data, isListLoading: isLoading || isFetching, refetch};
};