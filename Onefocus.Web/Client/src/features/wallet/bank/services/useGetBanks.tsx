import { useQuery } from "@tanstack/react-query";
import bankApi from "../../apis/bankApi";
import GetBanksRequest from "../../apis/interfaces/bank/GetBanksRequest";

const useGetBanks = (request: GetBanksRequest) => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['bank', 'useGetBanks', request],
        queryFn: async () => {
            const apiResponse = await bankApi.getBanks(request);
            return apiResponse.value.banks;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetBanks;