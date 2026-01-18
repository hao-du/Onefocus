import { useMutation, useQueryClient } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import UpdateCurrencyExchangeRequest from '../../../apis/interfaces/transaction/currency-exchange/UpdateCurrencyExchangeRequest';

const useUpdateCurrencyExchange = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCurrencyExchangeRequest>({
        mutationFn: async (request) => {
            return await transactionApi.updateCurrencyExchange(request);
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

    return { updateCurrencyExchangeAsync: mutateAsync, isCurrencyExchangeUpdating: isPending };
};

export default useUpdateCurrencyExchange;