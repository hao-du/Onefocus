import { useMutation, useQueryClient } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import UpdatePeerTransferRequest from '../../../apis/interfaces/transaction/peer-transfer/UpdatePeerTransferRequest';

const useUpdatePeerTransfer = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdatePeerTransferRequest>({
        mutationFn: async (request) => {
            return await transactionApi.updatePeerTransfer(request);
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

    return { updatePeerTransferAsync: mutateAsync, isPeerTransferUpdating: isPending };
};

export default useUpdatePeerTransfer;