import { useState } from 'react';
import { useQuery } from '../../../../../shared/hooks';
import { getCurrencyExchangeByTransactionId } from '../../apis';

const useGetCurrencyExchangeByTransactionId = () => {
    const [currencyExchangeTransactionId, setCurrencyExchangeTransactionId] = useState<string | null>(null);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCurrencyExchangeByTransactionId-${currencyExchangeTransactionId}`],
        queryFn: async () => {
            if (!currencyExchangeTransactionId) return null;

            const apiResponse = await getCurrencyExchangeByTransactionId(currencyExchangeTransactionId);
            return apiResponse.value;
        },
        enabled: Boolean(currencyExchangeTransactionId)
    });

    return {currencyExchangeEntity: data, isCurrencyExchangeLoading: isLoading, setCurrencyExchangeTransactionId};
};

export default useGetCurrencyExchangeByTransactionId;