export default interface PageContextValue<TFilter> {
    isActiveComponent: (componentId: string) => boolean;
    openComponent: (componentId: string) => void;
    closeComponent: () => void;
    dataId?: string;
    setDataId: React.Dispatch<React.SetStateAction<string | undefined>>;
    filter: TFilter;
    setFilter: React.Dispatch<React.SetStateAction<TFilter>>;
    resetFilter: () => void;
    registerRefreshCallback: (cb: () => void) => void;
    requestRefresh?: () => void;
    hasAnyLoading: boolean;
    setLoadings: (loadings: Record<string, boolean>) => void;
}