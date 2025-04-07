import {useQuery} from "@tanstack/react-query";

import {useClient} from "../../infrastructure/hooks/client/useClient";
import {toUserEntities} from "../../infrastructure/modules/user/user.adapters";
import {getAllUsers} from "../../infrastructure/modules/user/user.api";

const useGetUser = () => {
    const {client} = useClient();

    const {data, isLoading} = useQuery({
        queryKey: ['useGetUser'],
        queryFn: async () => {
            const apiResponse = await getAllUsers(client);
            return toUserEntities(apiResponse.value);
        }
    });

    return { data, isLoading };
};

export default useGetUser;