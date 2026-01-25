/* eslint-disable @typescript-eslint/no-empty-object-type */

import { useCallback, useMemo, useState } from "react";
import PageContext from "./PageContext";
import { ChildrenProps } from "../../props/BaseProps";
import PageContextValue from "./PageContextValue";
import { getGuid } from "../../utils";

interface PageProviderProps extends ChildrenProps {
}

const PageProvider = <TFilter = unknown,>(props: PageProviderProps) => {
    const [currentComponentId, setCurrentComponentId] = useState<string>();
    const [dataId, setDataId] = useState<string>();
    const [filter, setFilter] = useState<TFilter | undefined>();
    const [pageLoadings, setPageLoadings] = useState<Record<string, boolean>>({});
    const [expandDrawerTrigger, setExpandDrawerTrigger] = useState<string>(getGuid());

    const isActiveComponent = useCallback((componentId: string) => {
        return currentComponentId == componentId;
    }, [currentComponentId]);

    const openComponent = useCallback((componentId: string) => {
        setExpandDrawerTrigger(getGuid());
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
        setLoadings,
        expandDrawerTrigger
    }), [isActiveComponent, openComponent, closeComponent, dataId, filter, resetFilter, hasAnyLoading, setLoadings, expandDrawerTrigger]);

    return (
        <PageContext.Provider value={value}>
            {props.children}
        </PageContext.Provider>
    );
};

export default PageProvider;