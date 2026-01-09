import { useQuery } from "@tanstack/react-query";
import bankApi from "../../apis/bankApi";

const useGetAllBanks = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['getAllBanks'],
        queryFn: async () => {
            const apiResponse = await bankApi.getAllBanks();
            return apiResponse.value.banks;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllBanks;