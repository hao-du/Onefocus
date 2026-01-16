import { useMutation } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import CreateUserRequest from "../../apis/interfaces/CreateUserRequest";
import CreateUserResponse from "../../apis/interfaces/CreateUserResponse";
import userApi from "../../apis/userApi";

const useCreateUser = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse<CreateUserResponse>, unknown, CreateUserRequest>({
        mutationFn: async (request) => {
            return await userApi.createUser(request);
        }
    });

    return { createAsync: mutateAsync, isCreating: isPending };
};

export default useCreateUser;