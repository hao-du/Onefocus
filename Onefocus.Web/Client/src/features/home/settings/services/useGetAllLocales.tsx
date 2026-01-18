import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import settingsApi from "../../apis/settingsApi";

const useGetAllLocales = () => {
    const [isFetched, setIsFetched] = useState<boolean>(false);

    const { data, isLoading, isFetching } = useQuery({
        queryKey: ['settings', 'useGetAllLocales'],
        queryFn: async () => {
            const apiResponse = await settingsApi.getAllLocaleOptions();
            setIsFetched(true);
            return apiResponse.value?.locales;
        },
        staleTime: isFetched ? Infinity : 0
    });

    return { locales: data, isLocalesLoading: isLoading || isFetching };
};

export default useGetAllLocales;