import { useState } from 'react';
import { useQuery } from '../../../../../shared/hooks';
import { getPeerTransferByTransactionId } from '../../apis';

const useGetPeerTransferByTransactionId = () => {
    const [peerTransferTransactionId, setPeerTransferTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetPeerTransferByTransactionId-${peerTransferTransactionId}`],
        queryFn: async () => {
            if (!peerTransferTransactionId) return null;

            const apiResponse = await getPeerTransferByTransactionId(peerTransferTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(peerTransferTransactionId)
    });

    return {peerTransferEntity: data, isPeerTransferLoading: isLoading, setPeerTransferTransactionId};
};

export default useGetPeerTransferByTransactionId;