/* eslint-disable @typescript-eslint/no-empty-object-type */

import { useCallback, useMemo, useRef, useState } from "react";
import PageContext from "./PageContext";
import { ChildrenProps } from "../../props/BaseProps";

interface PageProviderProps extends ChildrenProps {
}

const PageProvider = <TFilter,>(props: PageProviderProps) => {
    const [currentComponentId, setCurrentComponentId] = useState<string>();
    const [currentObjectId, setCurrentObjectId] = useState<string>();
    const [filter, setFilter] = useState<TFilter>({} as TFilter);

    const loading = useRef<boolean>(false);
    const refreshCallbackRef = useRef<(() => void) | null>(null);

    const registerRefreshCallback = useCallback((cb: () => void) => {
        refreshCallbackRef.current = cb;
    }, []);

    const isPageLoading = useCallback(() => {
        return loading.current;
    }, []);

    const setPageLoading = useCallback((value: boolean) => {
        loading.current = value
    }, []);

    const requestRefresh = useCallback(() => {
        refreshCallbackRef.current?.();
    }, []);

    const value = useMemo(() => ({
        currentComponentId,
        setCurrentComponentId,
        currentObjectId,
        setCurrentObjectId,
        filter,
        setFilter,
        registerRefreshCallback,
        requestRefresh,
        isPageLoading,
        setPageLoading
    }), [
        currentComponentId,
        currentObjectId,
        filter,
        registerRefreshCallback,
        requestRefresh,
        isPageLoading,
        setPageLoading
    ]);

    return (
        <PageContext.Provider value={value}>
            {props.children}
        </PageContext.Provider>
    );
};

export default PageProvider;