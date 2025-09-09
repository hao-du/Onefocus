import { ApiResponse, useMutation } from '../../../../../shared/hooks';
import { UpdatePeerTransferRequest, updatePeerTransfer } from '../../apis';

const useUpdatePeerTransfer = () => {
    const {mutateAsync, isPending} = useMutation<ApiResponse, unknown, UpdatePeerTransferRequest>({
        mutationFn: async (request) => {
            return await updatePeerTransfer(request);
        }
    });

    return {onUpdateAsync: mutateAsync, isUpdating: isPending};
};

export default useUpdatePeerTransfer ;