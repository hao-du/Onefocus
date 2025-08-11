import { useMutation as useTanstackMutation, UseMutationOptions as UseTanstackMutationOptions } from '@tanstack/react-query';

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
  });
}
export default useMutation;