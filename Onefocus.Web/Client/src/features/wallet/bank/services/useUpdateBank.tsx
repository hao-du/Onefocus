import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ApiResponse } from '../../../../shared/hooks';
import { updateBank, UpdateBankRequest } from '../apis';

const useUpdateBank = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await updateBank(request);
        },
        onSuccess: (_, variables) => {
            const keysToReset = [['getAllBanks'], [`useGetBankById-${variables.id}`]];
            keysToReset.forEach((key) => {
                queryClient.resetQueries({ queryKey: key, exact: true });
            });
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateBank;