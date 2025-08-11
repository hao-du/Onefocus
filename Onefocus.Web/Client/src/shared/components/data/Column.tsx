import { ColumnBodyOptions, Column as PiColumn } from 'primereact/column';

type ColumnProps<TValue> = {
    field? : string;
    header?: string;
    headerStyle?: React.CSSProperties;
    align?: 'left' | 'right' | 'center';
    alignHeader?: 'left' | 'right' | 'center';
    body?: React.ReactNode | ((data: TValue, options: ColumnBodyOptions) => React.ReactNode);
    dataType?: 'text' | 'numeric' | 'date' | string | undefined;
};

const Column = <TValue,> (props : ColumnProps<TValue>) => {
    return (
        <PiColumn
            field={props.field}
            header={props.header}
            headerStyle={props.headerStyle}
            align={props.align}
            alignHeader={props.alignHeader}
            body={props.body}
            dataType={props.dataType}
        />
    );
}

export default Column;