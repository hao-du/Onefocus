import { useMutation as useTanstackMutation, UseMutationOptions as UseTanstackMutationOptions } from '@tanstack/react-query';
import { AxiosError } from 'axios';

type UseMutationOptions<TData, TError, TVariables> = UseTanstackMutationOptions<TData, TError, TVariables> & {
  mutationFn: (variables: TVariables) => Promise<TData>;
};

const useMutation = <TData = unknown, TError = unknown, TVariables = void>(
  options: UseMutationOptions<TData, TError, TVariables>
) => {
  return useTanstackMutation({
    ...options,
    mutationFn: async (variables: TVariables) => {
      return await options.mutationFn(variables);
    },
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
export default useMutation;