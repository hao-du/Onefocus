import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {getAllUsers, toUserEntities} from '../../infrastructure/modules/user';

const useGetUser = () => {
    const {client} = useClient();

    const {data, isLoading} = useQuery({
        queryKey: ['useGetUser'],
        queryFn: async () => {
            const apiResponse = await getAllUsers(client);
            return toUserEntities(apiResponse.value);
        }
    });

    return {data, isLoading};
};

export default useGetUser;