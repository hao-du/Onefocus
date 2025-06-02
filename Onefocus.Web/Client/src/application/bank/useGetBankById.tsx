import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {
    getBankById,
    getBankByIdAdapter
} from '../../infrastructure/modules/bank';

export const useGetBankById = (id: string) => {
    const {client} = useClient();

    const {data, isLoading} = useQuery({
        queryKey: [`useGetBankById-${id}`],
        queryFn: async () => {
            const apiResponse = await getBankById(client, id);
            return getBankByIdAdapter().toBankEntity(apiResponse.value);
        }
    });

    return {bank: data, isLoading};
};