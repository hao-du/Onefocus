import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {useState} from 'react';
import {getCashFlowByTransactionId, getCashFlowByIdAdapter} from '../../infrastructure/modules/transaction';

export const useGetCashFlowByTransactionId = () => {
    const {client} = useClient();
    const [transactionId, setTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetTransactionById-${transactionId}`],
        queryFn: async () => {
            if (!transactionId) return null;

            const apiResponse = await getCashFlowByTransactionId(client, transactionId);
            return getCashFlowByIdAdapter().toCashFlowEntity(apiResponse.value);
        },
        enabled: Boolean(transactionId)
    });

    return {cashFlowEntity: data, isCashFlowLoading: isLoading, setTransactionId: setTransactionId};
};