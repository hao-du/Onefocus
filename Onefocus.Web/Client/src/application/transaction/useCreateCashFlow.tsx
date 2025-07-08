import {useMutation} from '@tanstack/react-query';
import {ApiResponse, useClient} from '../../infrastructure/hooks';
import {createCashFlow, CreateCashFlowRequest, CreateCashFlowResponse} from '../../infrastructure/modules/transaction';

export const useCreateCashFlow = () => {
    const {client} = useClient();

    const {mutateAsync, isPending} = useMutation<ApiResponse<CreateCashFlowResponse>, unknown, CreateCashFlowRequest>({
        mutationFn: async (request) => {
            return await createCashFlow(client, request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};