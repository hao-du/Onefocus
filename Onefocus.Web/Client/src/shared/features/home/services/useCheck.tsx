import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useClient } from '../../../hooks';
import { check } from '../apis';

const useCheck = () => {
    const { isClientReady } = useClient();
    const [isCheckDone, setIsCheckDone] = useState<boolean>(false);

    useQuery({
        queryKey: ['useCheck'],
        queryFn: async () => {
            const response = await check();
            if (response.status === 200) {
                setIsCheckDone(true);
            }
        },
        enabled: isClientReady,
        staleTime: Infinity
    });

    return { isCheckDone };
};

export default useCheck;