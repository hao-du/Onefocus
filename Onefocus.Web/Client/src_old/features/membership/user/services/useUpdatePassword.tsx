import { ApiResponse, useMutation } from '../../../../shared/hooks';
import { updatePassword, UpdatePasswordRequest } from '../apis';

const useUpdatePassword = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdatePasswordRequest>({
        mutationFn: async (request) => {
            return await updatePassword(request);
        },
    });

    return { onUpdatePasswordAsync: mutateAsync, isUpdatingPassword: isPending };
};

export default useUpdatePassword;