import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { updateCurrencyExchange, UpdateCurrencyExchangeRequest } from '../../apis';

const useUpdateCurrencyExchange = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateCurrencyExchangeRequest>({
        mutationFn: async (request) => {
            return await updateCurrencyExchange(request);
        }
    });

    return {onUpdateAsync: mutateAsync, isUpdating: isPending};
};

export default useUpdateCurrencyExchange;