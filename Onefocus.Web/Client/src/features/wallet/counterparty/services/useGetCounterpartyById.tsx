import { useQuery } from "@tanstack/react-query";
import counterpartyApi from "../../apis/counterpartyApi";

const useGetCounterpartyById = (counterpartyId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['counterparty', 'useGetCounterpartyById', counterpartyId],
        queryFn: async () => {
            if (!counterpartyId) return null;

            const apiResponse = await counterpartyApi.getCounterpartyById(counterpartyId);
            return apiResponse.value;
        },
        enabled: Boolean(counterpartyId)
    });

    return { counterparty: data, isCounterpartyLoading: isLoading };
};

export default useGetCounterpartyById;