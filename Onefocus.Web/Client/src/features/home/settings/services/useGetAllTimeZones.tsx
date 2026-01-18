import { useQuery } from "@tanstack/react-query";
import settingsApi from "../../apis/settingsApi";

const useGetAllTimeZones = () => {
    const { data, isLoading, isFetching } = useQuery({
        queryKey: ['settings', 'useGetAllTimeZones'],
        queryFn: async () => {
            const apiResponse = await settingsApi.getAllTimeZoneOptions();
            return apiResponse.value?.timeZones;
        }
    });

    return { timezones: data, isTimeZonesLoading: isLoading || isFetching };
};

export default useGetAllTimeZones;