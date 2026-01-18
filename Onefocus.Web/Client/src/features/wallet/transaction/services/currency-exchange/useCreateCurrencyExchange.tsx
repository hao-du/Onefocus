import { useMutation } from '@tanstack/react-query';
import transactionApi from '../../../apis/transactionApi';
import ApiResponse from '../../../../../shared/apis/interfaces/ApiResponse';
import CreateCurrencyExchangeResponse from '../../../apis/interfaces/transaction/currency-exchange/CreateCurrencyExchangeResponse';
import CreateCurrencyExchangeRequest from '../../../apis/interfaces/transaction/currency-exchange/CreateCurrencyExchangeRequest';

const useCreateCurrencyExchange = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCurrencyExchangeResponse>, unknown, CreateCurrencyExchangeRequest>({
        mutationFn: async (request) => {
            return await transactionApi.createCurrencyExchange(request);
        }
    });

    return { createCurrencyExchangeAsync: mutateAsync, isCurrencyExchangeCreating: isPending };
};

export default useCreateCurrencyExchange;