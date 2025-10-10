import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getUserById } from '../apis';

const useGetUserById = () => {
    const [userId, setUserId] = useState<string | undefined>(undefined);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetUserById-${userId}`],
        queryFn: async () => {
            if (!userId) return null;

            const apiResponse = await getUserById(userId);
            return apiResponse.value;
        },
        enabled: Boolean(userId)
    });

    return {entity: data, isEntityLoading: isLoading, setUserId};
};

export default useGetUserById;