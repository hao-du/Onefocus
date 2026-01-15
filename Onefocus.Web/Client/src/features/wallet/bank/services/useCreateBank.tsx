import { useMutation } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import bankApi from "../../apis/bankApi";
import CreateBankResponse from "../../apis/interfaces/bank/CreateBankResponse";
import CreateBankRequest from "../../apis/interfaces/bank/CreateBankRequest";


const useCreateBank = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateBankResponse>, unknown, CreateBankRequest>({
        mutationFn: async (request) => {
            return await bankApi.createBank(request);
        }
    });

    return { createAsync: mutateAsync, isCreating: isPending };
};

export default useCreateBank;