import { useQuery } from "@tanstack/react-query";
import bankApi from "../../apis/bankApi";

const useGetBankById = (bankId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['bank', 'useGetBankById', bankId],
        queryFn: async () => {
            if (!bankId) return null;

            const apiResponse = await bankApi.getBankById(bankId);
            return apiResponse.value;
        },
        enabled: Boolean(bankId)
    });

    return { entity: data, isEntityLoading: isLoading };
};

export default useGetBankById;