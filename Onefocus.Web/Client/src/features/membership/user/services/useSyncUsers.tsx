import { useMutation } from "@tanstack/react-query";
import userApi from "../../apis/userApi";
import ApiResponse from "../../../../shared/apis/interfaces/ApiResponse";

const useSyncUsers = () => {
    const { mutateAsync, isPending } = useMutation<ApiResponse>({
        mutationFn: async () => {
            return await userApi.syncUsers();
        },
    });

    return { syncAsync: mutateAsync, isSynching: isPending };
};

export default useSyncUsers;