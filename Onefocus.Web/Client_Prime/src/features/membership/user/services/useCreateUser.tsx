import { ApiResponse, useMutation } from '../../../../shared/hooks';
import {
    createUser,
    CreateUserRequest,
    CreateUserResponse
} from '../apis';

const useCreateUser = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateUserResponse>, unknown, CreateUserRequest>({
        mutationFn: async (request) => {
            return await createUser(request);
        }
    });

    return { onCreateAsync: mutateAsync, isCreating: isPending };
};

export default useCreateUser;