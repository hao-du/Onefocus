import {
    DataTable as PiDataTable,
    DataTableValueArray
} from 'primereact/datatable';
import {BaseProps} from '../../props/BaseProps';
import * as React from 'react';

type DataTableProps<TValue extends DataTableValueArray> = BaseProps & {
    value?: TValue | undefined;
    children: React.ReactNode;
}

export const DataTable = <TValue extends DataTableValueArray> (props : DataTableProps<TValue>) => {
    return (
        <PiDataTable
            value={props.value}
            className={props.className}
        >
            {props.children}
        </PiDataTable>
    );
};