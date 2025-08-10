import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getCashFlowByTransactionId } from '../apis';

const useGetCashFlowByTransactionId = () => {
    const [transactionId, setTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetTransactionById-${transactionId}`],
        queryFn: async () => {
            if (!transactionId) return null;

            const apiResponse = await getCashFlowByTransactionId(transactionId);
            return apiResponse.value;
        },
        enabled: Boolean(transactionId)
    });

    return {cashFlowEntity: data, isCashFlowLoading: isLoading, setTransactionId: setTransactionId};
};

export default useGetCashFlowByTransactionId;