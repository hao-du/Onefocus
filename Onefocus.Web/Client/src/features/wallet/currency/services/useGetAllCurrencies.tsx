import {useQuery} from '@tanstack/react-query';
import { getAllCurrencies } from '../apis';

const useGetAllCurrencies = () => {
    const {data,  isLoading, refetch, isFetching} = useQuery({
        queryKey: ['getAllCurrencies'],
        queryFn: async () => {
            const apiResponse = await getAllCurrencies();
            return apiResponse.value.currencies;
        }
    });

    return {entities: data, isListLoading: isLoading || isFetching, refetch};
};

export default useGetAllCurrencies;