import { useQuery } from "@tanstack/react-query";
import counterpartyApi from "../../apis/counterpartyApi";


const useGetAllCounterparties = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['counterparty', 'useGetAllCounterparties'],
        queryFn: async () => {
            const apiResponse = await counterpartyApi.getAllCounterparties();
            return apiResponse.value.counterparties;
        }
    });

    return { counterparties: data, isCounterpartiesLoading: isLoading || isFetching, refetch };
};

export default useGetAllCounterparties;