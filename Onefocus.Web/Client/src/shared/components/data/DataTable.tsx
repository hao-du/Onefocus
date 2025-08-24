import { DataTableHeaderTemplateType, DataTableRowEditCompleteEvent, DataTableRowEditEvent, DataTableRowEditSaveEvent, DataTableValueArray, DataTable as PiDataTable } from 'primereact/datatable';
import * as React from 'react';
import { BaseProps } from '../props';

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
    header: DataTableHeaderTemplateType<TValue>;
}

const DataTable = <TValue extends DataTableValueArray> (props : DataTableProps<TValue>) => {
    return (
        <PiDataTable
            value={props.value}
            className={props.className}
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
        >
            {props.children}
        </PiDataTable>
    );
};

export default DataTable;