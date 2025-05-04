import {useState} from 'react';
import {useQuery} from '@tanstack/react-query';
import {useClient} from '../../infrastructure/hooks';
import {check} from '../../infrastructure/modules/home';

const useCheck = () => {
    const {client, isClientReady} = useClient();
    const [isCheckDone, setIsCheckDone] = useState<boolean>(false);

    useQuery({
        queryKey: ['useCheck'],
        queryFn: async () => {
            const response = await check(client);
            if (response.status === 200) {
                setIsCheckDone(true);
            }
        },
        enabled: isClientReady,
        staleTime: Infinity
    });

    return {isCheckDone};
};

export default useCheck;
