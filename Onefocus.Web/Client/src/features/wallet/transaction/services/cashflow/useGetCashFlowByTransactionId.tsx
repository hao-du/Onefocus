import { useQuery } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';

const useGetCashFlowByTransactionId = (cashFlowTransactionId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['transaction', 'useGetTransactionById', cashFlowTransactionId],
        queryFn: async () => {
            if (!cashFlowTransactionId) return null;

            const apiResponse = await transactionApi.getCashFlowByTransactionId(cashFlowTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(cashFlowTransactionId)
    });

    return { cashFlow: data, isCashFlowLoading: isLoading };
};

export default useGetCashFlowByTransactionId;