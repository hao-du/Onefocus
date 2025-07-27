import {useQuery} from '@tanstack/react-query';
import {getBankById } from '../apis';
import {useState} from 'react';

const useGetBankById = () => {
    const [bankId, setbankId] = useState<string | undefined>(undefined);

    const {data, isLoading} = useQuery({
        queryKey: [`useGetBankById-${bankId}`],
        queryFn: async () => {
            if (!bankId) return null;

            const apiResponse = await getBankById(bankId);
            return apiResponse.value;
        },
        enabled: Boolean(bankId)
    });

    return {entity: data, isEntityLoading: isLoading, setBankId: setbankId};
};

export default useGetBankById;