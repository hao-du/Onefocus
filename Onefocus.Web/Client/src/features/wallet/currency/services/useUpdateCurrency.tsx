import { useMutation, useQueryClient } from "@tanstack/react-query";
import currencyApi from "../../apis/currencyApi";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdateCurrencyRequest from "../../apis/interfaces/currency/UpdateCurrencyRequest";


const useUpdateCurrency = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCurrencyRequest>({
        mutationFn: async (request) => {
            return await currencyApi.updateCurrency(request);
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ['currency', 'useGetAllCurrencies']
            });

            if (variables.id) {
                queryClient.invalidateQueries({
                    queryKey: ['currency', 'useGetCurrencyById', variables.id]
                });
            }
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateCurrency;