import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { createBankAccount, CreateBankAccountRequest, CreateBankAccountResponse } from '../../apis';

const useCreateBankAccount = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateBankAccountResponse>, unknown, CreateBankAccountRequest>({
        mutationFn: async (request) => {
            return await createBankAccount(request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};

export default useCreateBankAccount;