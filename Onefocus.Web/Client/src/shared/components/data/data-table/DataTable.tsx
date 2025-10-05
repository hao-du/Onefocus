import { DataTableHeaderTemplateType, DataTableRowEditCompleteEvent, DataTableRowEditEvent, DataTableRowEditSaveEvent, DataTableValueArray, DataTable as PiDataTable } from 'primereact/datatable';
import * as React from 'react';
import { BaseProps } from '../../props';
import { Column as PiColumn } from 'primereact/column';
import { useLocale } from '../../../hooks';
import { ColumnProps } from './Column';

type DataTableProps<TValue extends DataTableValueArray> = BaseProps & {
    value?: TValue | undefined;
    children: React.ReactNode;
    isPending?: boolean;
    editMode?: 'cell' | 'row' | string;
    dataKey?: string;
    onRowEditInit?: (event: DataTableRowEditEvent) => void;
    onRowEditCancel?: (event: DataTableRowEditEvent) => void;
    onRowEditSave?: (event: DataTableRowEditSaveEvent<TValue>) => void;
    onRowEditChange?(event: DataTableRowEditEvent): void;
    editingRows?: Record<string, boolean>;
    onRowEditComplete?(event: DataTableRowEditCompleteEvent): void;
    header?: DataTableHeaderTemplateType<TValue>;
    style?: React.CSSProperties;
    tableStyle?: React.CSSProperties;
    scrollType?: 'flex' | 'responsive';
}

const DataTable = <TValue extends DataTableValueArray>(props: DataTableProps<TValue>) => {
    const { translate } = useLocale();

    const processedChildren = React.Children.map(props.children, (child) => {
        if (!React.isValidElement(child)) return child;

        const { header, ...rest } = child.props as ColumnProps<TValue>;

        return (
            <PiColumn
                {...rest}
                header={translate(header)}
            />
        );
    });

    return (
        <PiDataTable
            value={props.value}
            className={`${props.className ?? ''} flex-auto overflow-auto`}
            style={props.style}
            loading={props.isPending}
            loadingIcon="pi pi-spinner pi-spin"
            emptyMessage={props.isPending ? null : "Nothing to show right now."}
            dataKey={props.dataKey}
            editMode={props.editMode}
            editingRows={props.editingRows}
            onRowEditChange={props.onRowEditChange}
            onRowEditInit={props.onRowEditInit}
            onRowEditCancel={props.onRowEditCancel}
            onRowEditSave={props.onRowEditSave}
            onRowEditComplete={props.onRowEditComplete}
            header={props.header}
            tableStyle={props.tableStyle}
            scrollable
            scrollHeight={props.scrollType ?? 'flex'}
            showGridlines
        >
            {processedChildren}
        </PiDataTable>
    );
};

export default DataTable;