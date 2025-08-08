import { useQuery as useTanstackQuery, UseQueryOptions as UseTanstackQueryOptions } from '@tanstack/react-query';

type UseQueryOptions<TData, TError> = UseTanstackQueryOptions<TData, TError> & {
  queryFn: () => Promise<TData>;
};

const useQuery = <TData = unknown, TError = unknown>(
  options: UseQueryOptions<TData, TError>
) => {
  return useTanstackQuery({
    ...options,
    queryFn: async () => {
      try {
        return await options.queryFn();
      } catch (error) {
        throw error;
      }
    },
  });
}

export default useQuery;