import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import bankApi from "../../apis/bankApi";

const useGetBankById = () => {
    const [bankId, setbankId] = useState<string | undefined>(undefined);

    const { data, isLoading } = useQuery({
        queryKey: [`useGetBankById-${bankId}`],
        queryFn: async () => {
            if (!bankId) return null;

            const apiResponse = await bankApi.getBankById(bankId);
            return apiResponse.value;
        },
        enabled: Boolean(bankId)
    });

    return { entity: data, isEntityLoading: isLoading, setBankId: setbankId };
};

export default useGetBankById;