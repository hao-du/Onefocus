import { useQuery } from "@tanstack/react-query";
import userApi from "../../apis/userApi";

const useGetUserById = (userId: string | undefined) => {
    const { data, isLoading } = useQuery({
        queryKey: ['user', 'useGetUserById', userId],
        queryFn: async () => {
            if (!userId) return null;

            const apiResponse = await userApi.getUserById(userId);
            return apiResponse.value;
        },
        enabled: Boolean(userId)
    });

    return { entity: data, isEntityLoading: isLoading };
};

export default useGetUserById;