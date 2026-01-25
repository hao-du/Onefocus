import { useMutation, useQueryClient } from "@tanstack/react-query";
import currencyApi from "../../apis/currencyApi";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import CreateCurrencyResponse from "../../apis/interfaces/currency/CreateCurrencyResponse";
import CreateCurrencyRequest from "../../apis/interfaces/currency/CreateCurrencyRequest";

const useCreateCurrency = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCurrencyResponse>, unknown, CreateCurrencyRequest>({
        mutationFn: async (request) => {
            return await currencyApi.createCurrency(request);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ['currency', 'useGetAllCurrencies']
            });
        }
    });

    return { createAsync: mutateAsync, isCreating: isPending };
};

export default useCreateCurrency;