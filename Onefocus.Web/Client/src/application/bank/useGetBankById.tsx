import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getBankById, getBankByIdAdapter} from '../../infrastructure/modules/bank';
import {useState} from 'react';

export const useGetBankById = () => {
    const {client} = useClient();
    const [bankId, setbankId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetBankById-${bankId}`],
        queryFn: async () => {
            if (!bankId) return null;

            const apiResponse = await getBankById(client, bankId);
            return getBankByIdAdapter().toBankEntity(apiResponse.value);
        },
        enabled: Boolean(bankId)
    });

    return {entity: data, isEntityLoading: isLoading, setBankId: setbankId};
};