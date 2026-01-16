import { useMutation, useQueryClient } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdateUserRequest from "../../apis/interfaces/UpdateUserRequest";
import userApi from "../../apis/userApi";

const useUpdateUser = () => {
    const queryClient = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateUserRequest>({
        mutationFn: async (request) => {
            return await userApi.updateUser(request);
        },
        onSuccess: (_, variables) => {
            queryClient.invalidateQueries({
                queryKey: ['user', 'useGetAllUsers']
            });

            if (variables.id) {
                queryClient.invalidateQueries({
                    queryKey: ['user', 'useGetUserById', variables.id]
                });
            }
        }
    });

    return { updateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateUser;