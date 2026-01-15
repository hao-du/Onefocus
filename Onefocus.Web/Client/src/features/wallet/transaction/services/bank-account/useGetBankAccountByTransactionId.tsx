import { useQuery } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';

const useGetBankAccountByTransactionId = (bankAccountTransactionId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['transaction', 'useGetTransactionById', bankAccountTransactionId],
        queryFn: async () => {
            if (!bankAccountTransactionId) return null;

            const apiResponse = await transactionApi.getBankAccountByTransactionId(bankAccountTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(bankAccountTransactionId)
    });

    return { bankAccountEntity: data, isBankAccountLoading: isLoading };
};

export default useGetBankAccountByTransactionId;