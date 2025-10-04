import { getSettingsByUserId } from '../../../../shared/features/home/apis';
import { useQuery } from '../../../../shared/hooks';

const useGetSettingsByUserId = () => {
    const {data, isLoading, isFetching} = useQuery({
        queryKey: [`getSettingsByUserId`],
        queryFn: async () => {
            const apiResponse = await getSettingsByUserId();
            return apiResponse.value;
        },
    });

    return {entity: data, isEntityLoading: isLoading || isFetching};
};

export default useGetSettingsByUserId;