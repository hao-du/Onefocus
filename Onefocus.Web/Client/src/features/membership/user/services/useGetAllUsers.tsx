import { useQuery } from "@tanstack/react-query";
import userApi from "../../apis/userApi";

const useGetAllUsers = () => {
    const { data, isLoading, refetch, isFetching } = useQuery({
        queryKey: ['user', 'useGetAllUsers'],
        queryFn: async () => {
            const apiResponse = await userApi.getAllUsers();
            return apiResponse.value.users;
        }
    });

    return { entities: data, isListLoading: isLoading || isFetching, refetch };
};

export default useGetAllUsers;