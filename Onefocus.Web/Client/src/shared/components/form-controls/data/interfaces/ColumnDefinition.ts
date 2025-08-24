import { ColumnBodyOptions } from "./ColumnBodyOptions";

export interface ColumnDefinition<TValue extends readonly unknown[]> {
    name: string;
    label: string;
    body?: React.ReactNode | ((data: TValue[number], options: ColumnBodyOptions) => React.ReactNode);
    align?: 'left' | 'right' | 'center';
    alignHeader?: 'left' | 'right' | 'center';
    isReadOnly?: boolean;
}