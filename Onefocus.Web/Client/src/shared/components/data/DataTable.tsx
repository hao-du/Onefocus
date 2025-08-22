import { DataTableRowEditEvent, DataTableRowEditSaveEvent, DataTableValueArray, DataTable as PiDataTable } from 'primereact/datatable';
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
    onRowEditSave?: (event: DataTableRowEditEvent) => void;
    onRowEditChange?(event: DataTableRowEditEvent): void;
    editingRows?: Record<string, boolean>;
}

const DataTable = <TValue extends DataTableValueArray> (props : DataTableProps<TValue>) => {
    const onRowEditInit = (event: DataTableRowEditEvent) => {
        if(props.onRowEditInit) props.onRowEditInit(event);
    };

    const onRowEditCancel = (event: DataTableRowEditEvent) => {
        if(props.onRowEditCancel) props.onRowEditCancel(event);
    };

    const onRowEditSave = (event: DataTableRowEditSaveEvent<TValue>) => {
        if(props.onRowEditSave) props.onRowEditSave(event);
    };

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
            onRowEditInit={onRowEditInit}
            onRowEditCancel={onRowEditCancel}
            onRowEditSave={onRowEditSave}
        >
            {props.children}
        </PiDataTable>
    );
};

export default DataTable;