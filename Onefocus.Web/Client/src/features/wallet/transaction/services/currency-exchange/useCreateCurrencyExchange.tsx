import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { createCurrencyExchange, CreateCurrencyExchangeRequest, CreateCurrencyExchangeResponse } from '../../apis';

const useCreateCurrencyExchange = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateCurrencyExchangeResponse>, unknown, CreateCurrencyExchangeRequest>({
        mutationFn: async (request) => {
            return await createCurrencyExchange(request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};

export default useCreateCurrencyExchange;