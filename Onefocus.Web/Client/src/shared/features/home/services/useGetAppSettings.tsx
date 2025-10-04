import { useClient, useQuery } from '../../../hooks';
import { getSettingsByUserId } from '../apis';

const useGetAppSettings = () => {
    const { isClientReady } = useClient();

    const { data, isSuccess, refetch } = useQuery({
        queryKey: [`useGetAppSettings`],
        queryFn: async () => {
            const apiResponse = await getSettingsByUserId();
            return apiResponse.value;
        },
        enabled: isClientReady,
        refetchOnReconnect: false,
        refetchOnMount: false,
        staleTime: Infinity
    });

    return { appSettings: data, isAppSettingsReady: isSuccess, refetchAppSettings: refetch };
};

export default useGetAppSettings;