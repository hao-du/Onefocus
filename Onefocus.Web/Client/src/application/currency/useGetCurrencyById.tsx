import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {
    getCurrencyById,
    getCurrencyByIdAdapter
} from '../../infrastructure/modules/currency';

const useGetCurrencyById = (id: string) => {
    const {client} = useClient();

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCurrencyById-${id}`],
        queryFn: async () => {
            const apiResponse = await getCurrencyById(client, id);
            return getCurrencyByIdAdapter().toCurrencyEntity(apiResponse.value);
        }
    });

    return {data, isLoading};
};

export default useGetCurrencyById;