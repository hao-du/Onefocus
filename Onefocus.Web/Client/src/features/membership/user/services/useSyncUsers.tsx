import { ApiResponse, useMutation } from '../../../../shared/hooks';
import { syncUsers } from '../apis';

const useSyncUsers = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse>({
        mutationFn: async () => {
            return await syncUsers();
        },
    });

    return { onSyncAsync: mutateAsync, isSynching: isPending };
};

export default useSyncUsers;