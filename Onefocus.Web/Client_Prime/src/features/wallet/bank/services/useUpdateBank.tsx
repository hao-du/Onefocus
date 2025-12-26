import { ApiResponse, useMutation, useQueryClient } from '../../../../shared/hooks';
import { updateBank, UpdateBankRequest } from '../apis';

const useUpdateBank = () => {
    const {resetQueries} = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await updateBank(request);
        },
        onSuccess: (_, variables) => {
            resetQueries(['getAllBanks', `useGetBankById-${variables.id}`]);
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateBank;