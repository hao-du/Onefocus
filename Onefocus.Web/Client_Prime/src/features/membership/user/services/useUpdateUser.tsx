import { ApiResponse, useMutation, useQueryClient } from '../../../../shared/hooks';
import { updateUser, UpdateUserRequest } from '../apis';

const useUpdateUser = () => {
    const {resetQueries} = useQueryClient();

    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdateUserRequest>({
        mutationFn: async (request) => {
            return await updateUser(request);
        },
        onSuccess: (_, variables) => {
            resetQueries(['getAllUsers', `useGetUserById-${variables.id}`]);
        }
    });

    return { onUpdateAsync: mutateAsync, isUpdating: isPending };
};

export default useUpdateUser;