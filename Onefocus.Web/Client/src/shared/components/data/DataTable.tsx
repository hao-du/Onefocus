import {DataTable as PiDataTable, DataTableValueArray} from 'primereact/datatable';
import { BaseProps } from '../props';
import * as React from 'react';

type DataTableProps<TValue extends DataTableValueArray> = BaseProps & {
    value?: TValue | undefined;
    children: React.ReactNode;
    isPending?: boolean;
}

const DataTable = <TValue extends DataTableValueArray> (props : DataTableProps<TValue>) => {
    return (
        <PiDataTable
            value={props.value}
            className={props.className}
            loading={props.isPending}
            loadingIcon="pi pi-spinner pi-spin"
            emptyMessage={props.isPending ? null : "Nothing to show right now."}
        >
            {props.children}
        </PiDataTable>
    );
};

export default DataTable;