import { useMutation } from '@tanstack/react-query';
import { ApiResponse } from '../../../../shared/hooks';
import {
    createBank,
    CreateBankRequest,
    CreateBankResponse
} from '../apis';

const useCreateBank = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateBankResponse>, unknown, CreateBankRequest>({
        mutationFn: async (request) => {
            return await createBank(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreateBank;