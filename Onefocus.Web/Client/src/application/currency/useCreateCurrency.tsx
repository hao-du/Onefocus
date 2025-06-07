import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    createCurrency,
    CreateCurrencyRequest,
} from '../../infrastructure/modules/currency';
import {CreateBankResponse} from '../../infrastructure/modules/bank/bank.interfaces';

export const useCreateCurrency = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateBankResponse>, unknown, CreateCurrencyRequest>({
        mutationFn: async (request) => {
            return await createCurrency(client, request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};