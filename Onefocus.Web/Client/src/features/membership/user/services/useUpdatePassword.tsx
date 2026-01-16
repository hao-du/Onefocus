import { useMutation } from "@tanstack/react-query";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";
import UpdatePasswordRequest from "../../apis/interfaces/UpdatePasswordRequest";
import userApi from "../../apis/userApi";

const useUpdatePassword = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse, unknown, UpdatePasswordRequest>({
        mutationFn: async (request) => {
            return await userApi.updatePassword(request);
        },
    });

    return { updatePasswordAsync: mutateAsync, isUpdatingPassword: isPending };
};

export default useUpdatePassword;