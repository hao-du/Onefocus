import {useMutation,useQueryClient} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {
    updateBank,
    UpdateBankRequest,
} from '../../infrastructure/modules/bank';

export const useUpdateBank = () => {
    const {client} = useClient();
    const queryClient = useQueryClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await updateBank(client, request);
        },
        onSuccess: (_, variables) => {
            const keysToReset = [['getAllBanks'], [`useGetBankById-${variables.id}`]];
            keysToReset.forEach((key) => {
                queryClient.resetQueries({ queryKey: key, exact: true });
            });
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending};
};