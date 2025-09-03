import { UniqueComponentId } from 'primereact/utils';
import { PropsWithChildren, useState } from 'react';
import { FieldArray, FieldArrayPath, FieldValues, Path, useFieldArray, UseFormReturn } from 'react-hook-form';
import { BaseProps } from '../props';
import { ColumnBodyOptions } from './interfaces';
import { Button } from '../controls';
import DataTable from './DataTable';
import Column from './Column';
type EditableTableProps<TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>> = BaseProps & PropsWithChildren & {
    tableName?: string
    isReadOnly?: boolean;
    isPending?: boolean;
    form: UseFormReturn<TFormInput>;
    path: TName;
    newRowValue: FieldArray<TFormInput>;
    style?: React.CSSProperties;
}

const EditableTable = <TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>>(props: EditableTableProps<TFormInput, TName>) => {
    const [originalRows, setOriginalRows] = useState<Record<string, FieldArray<TFormInput, TName>>>({});
    const [editingRows, setEditingRows] = useState<Record<string, boolean>>({});

    const { control, getValues, trigger } = props.form;
    const { fields, update, append, remove } = useFieldArray({
        control: control,
        name: props.path
    });

    const actionBodyTemplate = (rowData: FieldArray<TFormInput, TName>, options: ColumnBodyOptions) => {
        const rowId = (rowData as FieldArray<TFormInput, TName> & { rowId: string }).rowId;
        const isEditing = !!editingRows[rowId];

        if (isEditing) {
            // When editing: Accept & Cancel buttons
            return (
                <div className="flex gap-2 justify-center">
                    <Button
                        icon="pi pi-check"
                        rounded
                        text
                        severity="success"
                        onClick={async () => {
                            const rowPathPrefix = `${props.path}.${options.rowIndex}` as Path<TFormInput>;
                            const currentRow = getValues(rowPathPrefix);
                            const rowFields = Object.keys(currentRow).map(
                                (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                            );

                            const isValid = await trigger(rowFields);
                            if (!isValid) {
                                return;
                            }

                            const updatedItem = getValues(props.path as Path<TFormInput>);
                            update(options.rowIndex, updatedItem[options.rowIndex]);

                            setEditingRows((prev) => {
                                const updated = { ...prev };
                                delete updated[rowId];
                                return updated;
                            });
                        }}
                    />
                    <Button
                        icon="pi pi-times"
                        rounded
                        text
                        severity="danger"
                        onClick={async () => {
                            const rowPathPrefix = `${props.path}.${options.rowIndex}` as Path<TFormInput>;
                            const currentRow = getValues(rowPathPrefix);
                            const rowFields = Object.keys(currentRow).map(
                                (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                            );

                            const isValid = await trigger(rowFields);
                            if (!isValid) {
                                return;
                            }

                            const original = originalRows[rowId];
                            if (original) {
                                update(options.rowIndex, original);
                            }

                            setEditingRows((prev) => {
                                const updated = { ...prev };
                                delete updated[rowId];  
                                return updated;
                            });
                        }}
                    />
                    <Button
                        icon="pi pi-trash"
                        rounded
                        text
                        severity="danger"
                        onClick={() => {
                            remove(options.rowIndex);
                        }}
                    />
                </div>
            );
        }

        // When not editing: Edit & Delete buttons
        return (
            <div className="flex gap-2 justify-center">
                <Button
                    icon="pi pi-pencil"
                    rounded
                    text
                    severity="info"
                    onClick={() => {
                        const arrayItems = getValues(props.path as Path<TFormInput>);
                        setOriginalRows((prev) => ({
                            ...prev,
                            [rowId]: { ...arrayItems[options.rowIndex] }
                        }));

                        setEditingRows((prev) => ({
                            ...prev,
                            [rowId]: true
                        }));
                    }}
                />
                <Button
                    icon="pi pi-trash"
                    rounded
                    text
                    severity="danger"
                    onClick={() => {
                        remove(options.rowIndex);
                    }}
                />
            </div>
        );
    };

    return (
        <DataTable
            value={fields}
            editMode="row"
            dataKey="rowId"
            isPending={props.isPending}
            tableStyle={{ tableLayout: 'fixed', width: '100%' }}
            header={
                <div className="flex flex-wrap align-items-center justify-content-between gap-2">
                    <span className="text-xl text-900 font-bold">{props.tableName}</span>
                    <Button icon="pi pi-plus" rounded onClick={() => {
                        const newValue = props.newRowValue !== null
                            ? { ...props.newRowValue, rowId: UniqueComponentId() }
                            : {} as FieldArray<TFormInput, TName> & { rowId: string };
                        append(newValue);

                        setEditingRows((prev) => ({
                            ...prev,
                            [newValue.rowId]: true
                        }));
                    }} />
                </div>
            }
            editingRows={editingRows}
            onRowEditChange={async (e) => {
                setEditingRows(e.data);
            }}
            scrollType="responsive"
            style={props.style}
        >
            <Column
                body={actionBodyTemplate}
                bodyStyle={{ textAlign: 'center', verticalAlign: 'top' }}
                style={{width:'10rem'}}
            />
            {props.children}
        </DataTable>
    );
};

export default EditableTable;