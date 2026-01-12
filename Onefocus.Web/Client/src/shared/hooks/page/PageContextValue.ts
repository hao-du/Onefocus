export default interface PageContextValue<TFilter> {
    currentComponentId?: string;
    setCurrentComponentId: React.Dispatch<React.SetStateAction<string | undefined>>;
    currentObjectId?: string;
    setCurrentObjectId: React.Dispatch<React.SetStateAction<string | undefined>>;
    filter: TFilter;
    setFilter: React.Dispatch<React.SetStateAction<TFilter>>;
    registerRefreshCallback: (cb: () => void) => void;
    requestRefresh?: () => void;
    isPageLoading: () => boolean;
    setPageLoading: (value: boolean) => void;
}
