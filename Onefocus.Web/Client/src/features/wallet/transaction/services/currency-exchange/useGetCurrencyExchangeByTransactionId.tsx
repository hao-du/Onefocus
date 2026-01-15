import { useQuery } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';

const useGetCurrencyExchangeByTransactionId = (currencyExchangeTransactionId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['transaction', 'useGetTransactionById', currencyExchangeTransactionId],
        queryFn: async () => {
            if (!currencyExchangeTransactionId) return null;

            const apiResponse = await transactionApi.getCurrencyExchangeByTransactionId(currencyExchangeTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(currencyExchangeTransactionId)
    });

    return { currencyExchangeEntity: data, isCurrencyExchangeLoading: isLoading };
};

export default useGetCurrencyExchangeByTransactionId;