import { useMutation, useQueryClient } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import UpdateBankAccountRequest from '../../../apis/interfaces/transaction/bank-account/UpdateBankAccountRequest';

const useUpdateBankAccount = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateBankAccountRequest>({
        mutationFn: async (request) => {
            return await transactionApi.updateBankAccount(request);
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

export default useUpdateBankAccount;