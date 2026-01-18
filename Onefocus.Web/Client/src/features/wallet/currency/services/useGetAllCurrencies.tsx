import { useQuery } from "@tanstack/react-query";
import currencyApi from "../../apis/currencyApi";

const useGetAllCurrencies = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['currency', 'useGetAllCurrencies'],
        queryFn: async () => {
            const apiResponse = await currencyApi.getAllCurrencies();
            return apiResponse.value.currencies;
        }
    });

    return { currencies: data, isCurrenciesLoading: isLoading || isFetching, refetch };
};

export default useGetAllCurrencies;