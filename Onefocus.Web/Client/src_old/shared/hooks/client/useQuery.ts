import { useQuery as useTanstackQuery, UseQueryOptions as UseTanstackQueryOptions } from '@tanstack/react-query';
import { AxiosError } from 'axios';

type UseQueryOptions<TData, TError> = UseTanstackQueryOptions<TData, TError> & {
  queryFn: () => Promise<TData>;
};

const useQuery = <TData = unknown, TError = unknown>(
  options: UseQueryOptions<TData, TError>
) => {
  return useTanstackQuery({
    ...options,
    queryFn: async () => {
        return await options.queryFn();
    },
    refetchOnWindowFocus: false,
    throwOnError: false,
    retry: (failureCount, error) => {
      const axiosError = error as AxiosError;

      const status = axiosError.response?.status;
      if (status === 500 && failureCount < 3) {
        return true;
      }
      return false;
    },
  });
}

export default useQuery;