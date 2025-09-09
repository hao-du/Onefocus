import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { createPeerTransfer, CreatePeerTransferRequest, CreatePeerTransferResponse } from '../../apis';

const useCreatePeerTransfer = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse<CreatePeerTransferResponse>, unknown, CreatePeerTransferRequest>({
        mutationFn: async (request) => {
            return await createPeerTransfer(request);
        }
    });

    return {onCreateAsync: mutateAsync, isCreating: isPending};
};

export default useCreatePeerTransfer;