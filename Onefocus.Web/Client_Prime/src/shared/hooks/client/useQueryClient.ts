import { useQueryClient as useTanstackQueryClient } from '@tanstack/react-query';

const useQueryClient = () => {
  const queryClient = useTanstackQueryClient();

  const resetQueries = (key: string[], exact: boolean = true) => {
    queryClient.resetQueries({ queryKey: [key], exact: exact });
  }

  return {
    resetQueries
  }
}
export default useQueryClient;