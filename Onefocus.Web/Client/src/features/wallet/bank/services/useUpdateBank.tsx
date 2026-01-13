import { useMutation, useQueryClient } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdateBankRequest from "../../apis/interfaces/UpdateBankRequest";
import bankApi from "../../apis/bankApi";

const useUpdateBank = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateBankRequest>({
        mutationFn: async (request) => {
            return await bankApi.updateBank(request);
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ['bank', 'useGetBanks']
            });

            if (variables.id) {
                queryClient.invalidateQueries({
                    queryKey: ['bank', 'useGetBankById', variables.id]
                });
            }
        }
    });

    return { updateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateBank;