import { useQuery } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';

const useGetPeerTransferByTransactionId = (peerTransferTransactionId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['transaction', 'useGetTransactionById', peerTransferTransactionId],
        queryFn: async () => {
            if (!peerTransferTransactionId) return null;

            const apiResponse = await transactionApi.getPeerTransferByTransactionId(peerTransferTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(peerTransferTransactionId)
    });

    return { peerTransfer: data, isPeerTransferLoading: isLoading };
};

export default useGetPeerTransferByTransactionId;