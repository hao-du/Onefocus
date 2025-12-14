import { ApiResponse, useMutation, useQueryClient } from '../../../../shared/hooks';
import { updateCurrency, UpdateCurrencyRequest } from '../apis';

const useUpdateCurrency = () => {
    const { resetQueries } = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCurrencyRequest>({
        mutationFn: async (request) => {
            return await updateCurrency(request);
        },
        onSuccess: (_, variables) => {
            resetQueries(['getAllCurrencies', `useGetCurrencyById-${variables.id}`]);
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateCurrency;