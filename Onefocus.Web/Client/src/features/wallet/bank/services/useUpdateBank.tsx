import { useMutation, useQueryClient } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdateBankRequest from "../../apis/interfaces/UpdateBankRequest";
import bankApi from "../../apis/bankApi";

const useUpdateBank = () => {
    const { resetQueries } = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await bankApi.updateBank(request);
        },
        onSuccess: (_, variables) => {
            resetQueries({
                queryKey: ['getAllBanks', `useGetBankById-${variables.id}`],
                exact: true
            });
        }
    });

    return { updateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateBank;