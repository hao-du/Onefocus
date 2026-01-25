import { useMutation, useQueryClient } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import CreateCashFlowResponse from '../../../apis/interfaces/transaction/cashflow/CreateCashFlowResponse';
import CreateCashFlowRequest from '../../../apis/interfaces/transaction/cashflow/CreateCashFlowRequest';

const useCreateCashFlow = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCashFlowResponse>, unknown, CreateCashFlowRequest>({
        mutationFn: async (request) => {
            return await transactionApi.createCashFlow(request);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ['transaction', 'useGetTransactions']
            });
        }
    });

    return { createCashFlowAsync: mutateAsync, isCashFlowCreating: isPending };
};

export default useCreateCashFlow;