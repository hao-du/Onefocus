import { useMutation } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import CreatePeerTransferResponse from '../../../apis/interfaces/transaction/peer-transfer/CreatePeerTransferResponse';
import CreatePeerTransferRequest from '../../../apis/interfaces/transaction/peer-transfer/CreatePeerTransferRequest';

const useCreatePeerTransfer = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreatePeerTransferResponse>, unknown, CreatePeerTransferRequest>({
        mutationFn: async (request) => {
            return await transactionApi.createPeerTransfer(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreatePeerTransfer;