import { useMutation, useQueryClient } from "@tanstack/react-query";
import CreateCounterpartyResponse from "../../apis/interfaces/counterparty/CreateCounterpartyResponse";
import CreateCounterpartyRequest from "../../apis/interfaces/counterparty/CreateCounterpartyRequest";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import counterpartyApi from "../../apis/counterpartyApi";

const useCreateCounterparty = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateCounterpartyResponse>, unknown, CreateCounterpartyRequest>({
        mutationFn: async (request) => {
            return await counterpartyApi.createCounterparty(request);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ['counterparty', 'useGetAllCounterparties']
            });
        }
    });

    return { createAsync: mutateAsync, isCreating: isPending };
};

export default useCreateCounterparty;