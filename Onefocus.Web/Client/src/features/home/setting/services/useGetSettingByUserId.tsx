import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getSettingByUserId } from '../apis';

const useGetSettingByUserId = () => {
    const [settingId, setSettingId] = useState<string | undefined>(undefined);

    const {data, isLoading} = useQuery({
        queryKey: [`getSettingByUserId`],
        queryFn: async () => {
            if (!settingId) return null;

            const apiResponse = await getSettingByUserId();
            return apiResponse.value;
        },
        enabled: Boolean(settingId)
    });

    return {entity: data, isEntityLoading: isLoading, setSettingId: setSettingId};
};

export default useGetSettingByUserId;