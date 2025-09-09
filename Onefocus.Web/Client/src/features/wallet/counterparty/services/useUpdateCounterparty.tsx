import { ApiResponse, useMutation, useQueryClient } from '../../../../shared/hooks';
import { updateCounterparty, UpdateCounterpartyRequest } from '../apis';

const useUpdateCounterparty = () => {
    const { resetQueries } = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCounterpartyRequest>({
        mutationFn: async (request) => {
            return await updateCounterparty(request);
        },
        onSuccess: (_, variables) => {
            resetQueries(['getAllCounterparties', `useGetCounterpartyById-${variables.id}`]);
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateCounterparty;