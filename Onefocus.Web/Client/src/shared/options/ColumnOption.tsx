/* eslint-disable @typescript-eslint/no-explicit-any */
export interface ColumnOption<T> {
    title?: string;
    key?: string;
    dataIndex?: string;
    render?: (value: any, record: T, index: number) => React.ReactNode;
};