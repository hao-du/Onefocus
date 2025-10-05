import { DefaultError, QueryObserverResult, RefetchOptions } from '@tanstack/react-query';

export default interface SettingsContextValue<TData = unknown, TError = DefaultError> {
    settings?: TData;
    refetch: (options?: RefetchOptions) => Promise<QueryObserverResult<TData, TError>>;
    isSettingsReady?: boolean;
}