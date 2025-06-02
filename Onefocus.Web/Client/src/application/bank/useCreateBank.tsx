import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    createBank,
    CreateBankRequest,
} from '../../infrastructure/modules/bank';

export const useCreateBank = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, CreateBankRequest>({
        mutationFn: async (request) => {
            return await createBank(client, request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};