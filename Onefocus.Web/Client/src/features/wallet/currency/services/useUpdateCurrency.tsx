import {useMutation, useQueryClient} from '@tanstack/react-query';
import {ApiResponse} from '../../../../shared/hooks';
import { updateCurrency, UpdateCurrencyRequest } from '../apis';

const useUpdateCurrency = () => {
    const queryClient = useQueryClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateCurrencyRequest>({
        mutationFn: async (request) => {
            return await updateCurrency(request);
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

export default useUpdateCurrency;