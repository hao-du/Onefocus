import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { updateCashFlow, UpdateCashFlowRequest } from '../../apis';

const useUpdateCashFlow = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdateCashFlowRequest>({
        mutationFn: async (request) => {
            return await updateCashFlow(request);
        }
    });

    return {onUpdateAsync: mutateAsync, isUpdating: isPending};
};

export default useUpdateCashFlow;