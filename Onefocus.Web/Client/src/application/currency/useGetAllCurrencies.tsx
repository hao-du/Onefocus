import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getAllCurrencies, getAllCurrenciesAdapter} from '../../infrastructure/modules/currency';

const useGetAllCurrencies = () => {
    const {client} = useClient();

    const {data, isLoading} = useQuery({
        queryKey: ['getAllCurrencies'],
        queryFn: async () => {
            const apiResponse = await getAllCurrencies(client);
            return getAllCurrenciesAdapter().toCurrencyEntities(apiResponse.value);
        }
    });

    return {data, isLoading};
};

export default useGetAllCurrencies;