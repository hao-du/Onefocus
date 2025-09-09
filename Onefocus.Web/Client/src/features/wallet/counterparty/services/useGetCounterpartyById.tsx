import { useState } from 'react';
import { useQuery } from '../../../../shared/hooks';
import { getCounterpartyById } from '../apis';

const useGetCounterpartyById = () => {
    const [counterpartyId, setCounterpartyId] = useState<string | undefined>(undefined);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetCounterpartyById-${counterpartyId}`],
        queryFn: async () => {
            if (!counterpartyId) return null;

            const apiResponse = await getCounterpartyById(counterpartyId);
            return apiResponse.value;
        },
        enabled: Boolean(counterpartyId)
    });

    return {entity: data, isEntityLoading: isLoading, setCounterpartyId};
};

export default useGetCounterpartyById;