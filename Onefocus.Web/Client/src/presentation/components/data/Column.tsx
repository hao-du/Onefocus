import {Column as PiColumn, ColumnBodyOptions} from 'primereact/column';

export type ColumnProps<TValue> = {
    field? : string;
    header?: string;
    headerStyle?: React.CSSProperties;
    body?: React.ReactNode | ((data: TValue, options: ColumnBodyOptions) => React.ReactNode);
};

export const Column = <TValue,> (props : ColumnProps<TValue>) => {
    return (
        <PiColumn
            field={props.field}
            header={props.header}
            headerStyle={props.headerStyle}
            body={props.body}
        />
    );
}