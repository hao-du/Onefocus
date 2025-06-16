import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {useState} from 'react';
import {getCashFlowById, getCashFlowByIdAdapter} from '../../infrastructure/modules/transaction';

export const useGetCashFlowById = () => {
    const {client} = useClient();
    const [cashFlowId, setCashFlowId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetTransactionById-${cashFlowId}`],
        queryFn: async () => {
            if (!cashFlowId) return null;

            const apiResponse = await getCashFlowById(client, cashFlowId);
            return getCashFlowByIdAdapter().toCashFlowEntity(apiResponse.value);
        },
        enabled: Boolean(cashFlowId)
    });

    return {cashFlowEntity: data, isCashFlowLoading: isLoading, setCashFlowId: setCashFlowId};
};