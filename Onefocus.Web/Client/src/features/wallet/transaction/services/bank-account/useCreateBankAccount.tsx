
import { useMutation } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import CreateBankAccountResponse from '../../../apis/interfaces/transaction/bank-account/CreateBankAccountResponse';
import CreateBankAccountRequest from '../../../apis/interfaces/transaction/bank-account/CreateBankAccountRequest';

const useCreateBankAccount = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateBankAccountResponse>, unknown, CreateBankAccountRequest>({
        mutationFn: async (request) => {
            return await transactionApi.createBankAccount(request);
        }
    });

    return { createBankAccountAsync: mutateAsync, isBankAccountCreating: isPending };
};

export default useCreateBankAccount;