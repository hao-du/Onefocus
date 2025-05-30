import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    updateCurrency,
    UpdateCurrencyRequest,
} from '../../infrastructure/modules/currency';

const useUpdateCurrency = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateCurrencyRequest>({
        mutationFn: async (request) => {
            return await updateCurrency(client, request);
        }
    });

    return {mutateAsync, isPending};
};

export default useUpdateCurrency;