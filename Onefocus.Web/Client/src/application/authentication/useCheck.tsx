import {useQuery} from "@tanstack/react-query";
import {useClient} from "../../infrastructure/hooks/client/useClient";
import {check} from "../../infrastructure/modules/authentication/authentication.api";
import {useState} from "react";

const useCheck = () => {
    const {client, isClientReady} = useClient();
    const [isCheckDone, setIsCheckDone] = useState<boolean>(false);

    useQuery({
        queryKey: ['useCheck'],
        queryFn: async () => {
            const apiResponse = await check(client);
            setIsCheckDone(true);
        },
        enabled: isClientReady
    });

    return { isCheckDone };
};

export default useCheck;
