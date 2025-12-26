import { ApiResponse, useMutation } from '../../../../shared/hooks';
import { createCurrency, CreateCurrencyRequest, CreateCurrencyResponse } from '../apis';

const useCreateCurrency = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCurrencyResponse>, unknown, CreateCurrencyRequest>({
        mutationFn: async (request) => {
            return await createCurrency(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreateCurrency;