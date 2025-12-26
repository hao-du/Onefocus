import { ApiResponse, useMutation } from '../../../../shared/hooks';
import {
    upsertSettings,
    UpsertSettingsRequest,
} from '../apis';

const useUpsertSettings = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpsertSettingsRequest>({
        mutationFn: async (request) => {
            return await upsertSettings(request);
        }
    });

    return { onUpsertAsync: mutateAsync, isUpserting: isPending };
};

export default useUpsertSettings;