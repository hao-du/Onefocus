import { ApiResponse, useMutation } from '../../../../shared/hooks';
import {
    createCounterparty,
    CreateCounterpartyRequest,
    CreateCounterpartyResponse
} from '../apis';

const useCreateCounterparty = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCounterpartyResponse>, unknown, CreateCounterpartyRequest>({
        mutationFn: async (request) => {
            return await createCounterparty(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreateCounterparty;