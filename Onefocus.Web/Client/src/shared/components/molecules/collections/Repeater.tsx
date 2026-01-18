

import { ReactNode } from "react";

interface RepeaterProps<TDataSource> {
    dataSource?: TDataSource[],
    body?: (record: TDataSource) => ReactNode;
}

const Repeater = <TDataSource,>(props: RepeaterProps<TDataSource>) => {
    if (!props.dataSource) return null;
    return (
        <>
            {props.dataSource.map((record) => {
                return props.body?.(record);
            })}
        </>
    );
};

export default Repeater;