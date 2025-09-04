import { useState } from 'react';
import { useQuery } from '../../../../../shared/hooks';
import { getBankAccountByTransactionId } from '../../apis';

const useGetBankAccountByTransactionId = () => {
    const [bankAccountTransactionId, setBankAccountTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetBankAccountByTransactionId-${bankAccountTransactionId}`],
        queryFn: async () => {
            if (!bankAccountTransactionId) return null;

            const apiResponse = await getBankAccountByTransactionId(bankAccountTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(bankAccountTransactionId)
    });

    return {bankAccountEntity: data, isBankAccountLoading: isLoading, setBankAccountTransactionId};
};

export default useGetBankAccountByTransactionId;