import {useMutation, useQueryClient} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    updateCurrency,
    UpdateCurrencyRequest,
} from '../../infrastructure/modules/currency';

export const useUpdateCurrency = () => {
    const {client} = useClient();
    const queryClient = useQueryClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateCurrencyRequest>({
        mutationFn: async (request) => {
            return await updateCurrency(client, request);
        },
        onSuccess: (_, variables) => {
            const keysToReset = [['getAllCurrencies'], [`useGetCurrencyById-${variables.id}`]];
            keysToReset.forEach((key) => {
                queryClient.resetQueries({ queryKey: key, exact: true });
            });
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending};
};