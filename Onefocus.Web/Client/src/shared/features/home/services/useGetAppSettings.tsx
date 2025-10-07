import { useState } from 'react';
import { useQuery } from '../../../hooks';
import { getSettingsByUserId } from '../apis';

const useGetAppSettings = () => {
    const [isFetched, setIsFetched] = useState<boolean>(false);

    const { data, isSuccess, refetch } = useQuery({
        queryKey: [`useGetAppSettings`],
        queryFn: async () => {
            if (!isFetched) {
                const apiResponse = await getSettingsByUserId();
                setIsFetched(true);
                return apiResponse.value;
            }
        },
        enabled: false,
        refetchOnReconnect: false,
        refetchOnMount: false,
        staleTime: Infinity,
        retry: false,
    });

    return { appSettings: data, isAppSettingsReady: isFetched || isSuccess, refetchAppSettings: refetch };
};

export default useGetAppSettings;