import { useQuery } from "@tanstack/react-query";
import currencyApi from "../../apis/currencyApi";

const useGetCurrencyById = (currencyId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['currency', 'useGetCurrencyById', currencyId],
        queryFn: async () => {
            if (!currencyId) return null;

            const apiResponse = await currencyApi.getCurrencyById(currencyId);
            return apiResponse.value;
        },
        enabled: Boolean(currencyId)
    });

    return { currency: data, isCurrencyLoading: isLoading };
};

export default useGetCurrencyById;