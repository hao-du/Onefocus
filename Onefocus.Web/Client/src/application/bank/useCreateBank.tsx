import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    createBank,
    CreateBankRequest,
} from '../../infrastructure/modules/bank';
import {CreateBankResponse} from '../../infrastructure/modules/bank/bank.interfaces';

export const useCreateBank = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateBankResponse>, unknown, CreateBankRequest>({
        mutationFn: async (request) => {
            return await createBank(client, request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};