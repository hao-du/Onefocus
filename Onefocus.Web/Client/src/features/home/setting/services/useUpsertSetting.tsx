import { ApiResponse, useMutation } from '../../../../shared/hooks';
import {
    upsertSetting,
    UpsertSettingRequest,
} from '../apis';

const useUpsertSetting = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpsertSettingRequest>({
        mutationFn: async (request) => {
            return await upsertSetting(request);
        }
    });

    return { onUpsertAsync: mutateAsync, isUpserting: isPending };
};

export default useUpsertSetting;