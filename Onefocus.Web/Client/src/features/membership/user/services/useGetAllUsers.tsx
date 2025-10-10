
import { useQuery } from '../../../../shared/hooks';
import { getAllUsers } from '../apis';

const useGetAllUsers = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['getAllUsers'],
        queryFn: async () => {
            const apiResponse = await getAllUsers();
            return apiResponse.value.users;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllUsers;