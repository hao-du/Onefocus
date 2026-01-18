import { useMutation } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpsertSettingsRequest from "../../apis/interfaces/settings/UpsertSettingsRequest";
import settingsApi from "../../apis/settingsApi";

const useUpsertSettings = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpsertSettingsRequest>({
        mutationFn: async (request) => {
            return await settingsApi.upsertSettings(request);
        }
    });

    return { upsertAsync: mutateAsync, isUpserting: isPending };
};

export default useUpsertSettings;