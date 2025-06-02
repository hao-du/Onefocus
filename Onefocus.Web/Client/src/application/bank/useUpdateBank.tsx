import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    updateBank,
    UpdateBankRequest,
} from '../../infrastructure/modules/bank';

export const useUpdateBank = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await updateBank(client, request);
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending};
};