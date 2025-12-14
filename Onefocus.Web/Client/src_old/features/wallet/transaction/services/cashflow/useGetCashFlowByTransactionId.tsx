import { useState } from 'react';
import { useQuery } from '../../../../../shared/hooks';
import { getCashFlowByTransactionId } from '../../apis';

const useGetCashFlowByTransactionId = () => {
    const [cashFlowTransactionId, setCashFlowTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCashFlowByTransactionId-${cashFlowTransactionId}`],
        queryFn: async () => {
            if (!cashFlowTransactionId) return null;

            const apiResponse = await getCashFlowByTransactionId(cashFlowTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(cashFlowTransactionId)
    });

    return {cashFlowEntity: data, isCashFlowLoading: isLoading, setCashFlowTransactionId};
};

export default useGetCashFlowByTransactionId;