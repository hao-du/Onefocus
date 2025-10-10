import { useQuery } from '../../../hooks';
import { getSettingsByUserId } from '../apis';

const useGetAppSettings = () => {
    const { data, isSuccess, refetch } = useQuery({
        queryKey: [`useGetAppSettings`],
        queryFn: async () => {
            const apiResponse = await getSettingsByUserId();
            return apiResponse.value; 
        },
        enabled: false,
        refetchOnReconnect: false,
        refetchOnMount: false,
        staleTime: Infinity,
        retry: false,
    });

    return { appSettings: data, isAppSettingsReady: isSuccess, refetchAppSettings: refetch };
};

export default useGetAppSettings;