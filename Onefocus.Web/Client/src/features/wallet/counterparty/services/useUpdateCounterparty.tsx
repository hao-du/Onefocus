import { useMutation, useQueryClient } from "@tanstack/react-query";
import counterpartyApi from "../../apis/counterpartyApi";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdateCounterpartyRequest from "../../apis/interfaces/counterparty/UpdateCounterpartyRequest";

const useUpdateCounterparty = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateCounterpartyRequest>({
        mutationFn: async (request) => {
            return await counterpartyApi.updateCounterparty(request);
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ['counterparty', 'useGetAllCounterparties']
            });

            if (variables.id) {
                queryClient.invalidateQueries({
                    queryKey: ['counterparty', 'useGetCounterpartyById', variables.id]
                });
            }
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateCounterparty;