import { useMutation } from "@tanstack/react-query";
import currencyApi from "../../apis/currencyApi";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import CreateCurrencyResponse from "../../apis/interfaces/currency/CreateCurrencyResponse";
import CreateCurrencyRequest from "../../apis/interfaces/currency/CreateCurrencyRequest";

const useCreateCurrency = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCurrencyResponse>, unknown, CreateCurrencyRequest>({
        mutationFn: async (request) => {
            return await currencyApi.createCurrency(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreateCurrency;