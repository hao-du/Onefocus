import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    createCurrency,
    CreateCurrencyRequest,
} from '../../infrastructure/modules/currency';

const useCreateCurrency = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, CreateCurrencyRequest>({
        mutationFn: async (request) => {
            return await createCurrency(client, request);
        }
    });

    return {mutateAsync, isPending};
};

export default useCreateCurrency;