import { ColumnEditorOptions, Column as PiColumn } from 'primereact/column';
import { ColumnBodyOptions } from './interfaces';
import { useLocale } from '../../../hooks';
import { BaseProps } from '../../props';

export type ColumnProps<TValue> = BaseProps & {
    field?: string;
    header?: string;
    style?: React.CSSProperties;
    headerStyle?: React.CSSProperties;
    bodyStyle?: React.CSSProperties;
    align?: 'left' | 'right' | 'center';
    alignHeader?: 'left' | 'right' | 'center';
    body?: React.ReactNode | ((data: TValue, options: ColumnBodyOptions) => React.ReactNode);
    dataType?: 'text' | 'numeric' | 'date' | string | undefined;
    editor?: React.ReactNode | ((options: ColumnEditorOptions) => React.ReactNode);
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    rowEditor?: boolean | ((data: any, options: ColumnBodyOptions) => boolean);
};

const Column = <TValue,>(props: ColumnProps<TValue>) => {
    const { translate } = useLocale();

    return (
        <PiColumn
            field={props.field}
            header={translate(props.header)}
            style={props.style}
            bodyStyle={props.bodyStyle}
            headerStyle={props.headerStyle}
            align={props.align}
            alignHeader={props.alignHeader}
            body={props.body}
            dataType={props.dataType}
            editor={props.editor}
            rowEditor={props.rowEditor}
        />
    );
}

export default Column;