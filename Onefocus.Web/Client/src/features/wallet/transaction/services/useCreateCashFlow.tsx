import { ApiResponse, useMutation } from '../../../../shared/hooks';
import { createCashFlow, CreateCashFlowRequest, CreateCashFlowResponse } from '../apis';

const useCreateCashFlow = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateCashFlowResponse>, unknown, CreateCashFlowRequest>({
        mutationFn: async (request) => {
            return await createCashFlow(request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};

export default useCreateCashFlow;