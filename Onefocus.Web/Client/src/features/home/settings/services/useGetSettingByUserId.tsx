
import { useQuery } from '@tanstack/react-query';
import homeApi from '../../../../shared/apis/homeApi';

const useGetSettingsByUserId = () => {
    const { data, isLoading, isFetching } = useQuery({
        queryKey: ['settings', 'getSettingsByUserId'],
        queryFn: async () => {
            const apiResponse = await homeApi.getSettingsByUserId();
            return apiResponse.value;
        },
    });

    return { userSettings: data, isUserSettingsLoading: isLoading || isFetching };
};

export default useGetSettingsByUserId;