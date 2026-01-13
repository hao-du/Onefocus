/* eslint-disable @typescript-eslint/no-empty-object-type */

import { useCallback, useMemo, useRef, useState } from "react";
import PageContext from "./PageContext";
import { ChildrenProps } from "../../props/BaseProps";
import PageContextValue from "./PageContextValue";

interface PageProviderProps extends ChildrenProps {
}

const PageProvider = <TFilter,>(props: PageProviderProps) => {
    const [currentComponentId, setCurrentComponentId] = useState<string>();
    const [dataId, setDataId] = useState<string>();
    const [filter, setFilter] = useState<TFilter>({} as TFilter);
    const [pageLoadings, setPageLoadings] = useState<Record<string, boolean>>({});
    const refreshCallbackRef = useRef<(() => void) | null>(null);

    const isActiveComponent = useCallback((componentId: string) => {
        return currentComponentId == componentId;
    }, [currentComponentId]);

    const openComponent = useCallback((componentId: string) => {
        setCurrentComponentId(componentId);
    }, []);

    const closeComponent = useCallback(() => {
        setCurrentComponentId(undefined);
    }, []);

    const registerRefreshCallback = useCallback((cb: () => void) => {
        refreshCallbackRef.current = cb;
    }, []);

    const requestRefresh = useCallback(() => {
        refreshCallbackRef.current?.();
    }, []);

    const setLoadings = useCallback((loadings: Record<string, boolean>) => {
        setPageLoadings(prev => ({
            ...prev,
            ...loadings
        }));
    }, []);

    const hasAnyLoading = useMemo(() => {
        return Object.values(pageLoadings).some(Boolean);
    }, [pageLoadings]);

    const resetFilter = useCallback(() => {
        setFilter({} as TFilter);
    }, []);

    const value = useMemo<PageContextValue<TFilter>>(() => ({
        isActiveComponent,
        openComponent,
        closeComponent,
        dataId,
        setDataId,
        filter,
        setFilter,
        resetFilter,
        registerRefreshCallback,
        requestRefresh,
        hasAnyLoading,
        setLoadings
    }), [isActiveComponent, openComponent, closeComponent, dataId, filter, resetFilter, registerRefreshCallback, requestRefresh, hasAnyLoading, setLoadings]);

    return (
        <PageContext.Provider value={value}>
            {props.children}
        </PageContext.Provider>
    );
};

export default PageProvider;