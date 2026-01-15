import { useMutation, useQueryClient } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import UpdateCashFlowRequest from '../../../apis/interfaces/transaction/cashflow/UpdateCashFlowRequest';

const useUpdateCashFlow = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCashFlowRequest>({
        mutationFn: async (request) => {
            return await transactionApi.updateCashFlow(request);
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ['transaction', 'useGetTransactions']
            });

            if (variables.id) {
                queryClient.invalidateQueries({
                    queryKey: ['transaction', 'useGetTransactionById', variables.id]
                });
            }
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateCashFlow;