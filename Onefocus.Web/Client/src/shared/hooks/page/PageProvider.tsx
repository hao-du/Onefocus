/* eslint-disable @typescript-eslint/no-empty-object-type */

import { useCallback, useMemo, useState } from "react";
import PageContext from "./PageContext";
import { ChildrenProps } from "../../props/BaseProps";
import PageContextValue from "./PageContextValue";

interface PageProviderProps extends ChildrenProps {
}

const PageProvider = <TFilter = unknown,>(props: PageProviderProps) => {
    const [currentComponentId, setCurrentComponentId] = useState<string>();
    const [dataId, setDataId] = useState<string>();
    const [filter, setFilter] = useState<TFilter | undefined>();
    const [pageLoadings, setPageLoadings] = useState<Record<string, boolean>>({});

    const isActiveComponent = useCallback((componentId: string) => {
        return currentComponentId == componentId;
    }, [currentComponentId]);

    const openComponent = useCallback((componentId: string) => {
        setCurrentComponentId(componentId);
    }, []);

    const closeComponent = useCallback(() => {
        setDataId(undefined);
        setCurrentComponentId(undefined);
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
        setFilter(undefined);
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
        hasAnyLoading,
        setLoadings
    }), [isActiveComponent, openComponent, closeComponent, dataId, filter, resetFilter, hasAnyLoading, setLoadings]);

    return (
        <PageContext.Provider value={value}>
            {props.children}
        </PageContext.Provider>
    );
};

export default PageProvider;