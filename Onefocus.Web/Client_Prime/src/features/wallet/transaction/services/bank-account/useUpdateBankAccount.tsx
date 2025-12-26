import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { UpdateBankAccountRequest, updateBankAccount } from '../../apis';

const useUpdateBankAccount = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateBankAccountRequest>({
        mutationFn: async (request) => {
            return await updateBankAccount(request);
        }
    });

    return {onUpdateAsync: mutateAsync, isUpdating: isPending};
};

export default useUpdateBankAccount ;