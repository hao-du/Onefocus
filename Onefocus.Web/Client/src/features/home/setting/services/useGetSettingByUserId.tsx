import { useQuery } from '../../../../shared/hooks';
import { getSettingByUserId } from '../apis';

const useGetSettingByUserId = () => {
    const {data, isLoading, isFetching} = useQuery({
        queryKey: [`getSettingByUserId`],
        queryFn: async () => {
            const apiResponse = await getSettingByUserId();
            return apiResponse.value;
        },
    });

    return {entity: data, isEntityLoading: isLoading || isFetching};
};

export default useGetSettingByUserId;