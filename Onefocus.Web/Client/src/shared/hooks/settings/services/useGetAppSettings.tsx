import { useQuery } from '@tanstack/react-query';
import homeApi from '../../../apis/homeApis';

const useGetAppSettings = () => {
    const { data, isSuccess, refetch } = useQuery({
        queryKey: [`useGetAppSettings`],
        queryFn: async () => {
            const apiResponse = await homeApi.getSettingsByUserId();
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