import { useState } from 'react';
import { FieldArray, FieldArrayPath, FieldValues, Path, useFieldArray, UseFormReturn } from 'react-hook-form';
import { Button } from '../../controls';
import { Column, DataTable, DataTableValueArray } from '../../data';
import { BaseProps } from '../../props';
import { Text } from '../inputs';

type EditableTableProps<TFormInput extends FieldValues, TFields extends DataTableValueArray, TName extends FieldArrayPath<TFormInput>> = BaseProps & {
    tableName?: string
    isReadOnly?: boolean;
    isPending?: boolean;
    form: UseFormReturn<TFormInput>;
    path: TName;
    newRowValue: FieldArray<TFormInput>;
}

const EditableTable = <TFormInput extends FieldValues, TFields extends DataTableValueArray, TName extends FieldArrayPath<TFormInput>>(props: EditableTableProps<TFormInput, TFields, TName>) => {
    const [originalRows, setOriginalRows] = useState<Record<string, FieldArray<TFormInput, TName>>>({});
    const [editingRows, setEditingRows] = useState<Record<string, boolean>>({});

    const { control, getValues, trigger } = props.form;
    const { fields, update, append, remove } = useFieldArray({
        control: control,
        name: props.path
    });

    return (
        <DataTable
            value={fields}
            editMode="row"
            dataKey="rowId"
            isPending={props.isPending}
            header={
                <div className="flex flex-wrap align-items-center justify-content-between gap-2">
                    <span className="text-xl text-900 font-bold">{props.tableName}</span>
                    <Button icon="pi pi-plus" rounded onClick={() => {
                        append(props.newRowValue);
                    }} />
                </div>
            }
            editingRows={editingRows}
            onRowEditChange={(e) => setEditingRows(e.data)}
            onRowEditComplete={async (e) => {
                const path = `${props.path}.${e.index}.name` as Path<TFormInput>;
                const isValid = await trigger(path);
                if (!isValid) {
                    setEditingRows((prev) => ({
                        ...prev,
                        [e.data.rowId]: true,
                    }));
                    return;
                }

                const updatedItem = getValues(props.path as Path<TFormInput>);
                update(e.index, updatedItem[e.index]);

                // Remove this row from editing state
                const newEditingRows = { ...editingRows };
                delete newEditingRows[e.data.rowId];
                setEditingRows(newEditingRows);
            }}
            onRowEditInit={(e) => {
                const arrayItems = getValues(props.path as Path<TFormInput>);
                setOriginalRows((prev) => ({
                    ...prev,
                    [e.data.rowId]: { ...arrayItems[e.index] }
                }));
            }}
            onRowEditCancel={async (e) => {
                const path = `${props.path}.${e.index}.name` as Path<TFormInput>;
                const isValid = await trigger(path);
                if (!isValid) {
                    setEditingRows((prev) => ({
                        ...prev,
                        [e.data.rowId]: true,
                    }));
                    return;
                }

                const original = originalRows[e.data.rowId];
                if (original) {
                    update(e.index, original);
                }

                // Remove this row from editing state
                const newEditingRows = { ...editingRows };
                delete newEditingRows[e.data.rowId];
                setEditingRows(newEditingRows);
            }}
        >
            <Column field="name" header="Name" editor={(options) => {
                return (
                    <Text
                        name={`${props.path}.${options.rowIndex}.name` as Path<TFormInput>}
                        control={control}
                        label="Name"
                        className="w-full of-w-max"
                        rules={{
                            required: 'Name is required.',
                            maxLength: { value: 100, message: 'Name cannot exceed 100 characters.' }
                        }}
                    />);
            }} />
            <Column
                rowEditor
                headerStyle={{ width: '10%', minWidth: '8rem' }}
                bodyStyle={{ textAlign: 'center' }}
            />
            <Column
                body={(_, { rowIndex }) => (
                    <Button
                        icon="pi pi-trash"
                        onClick={() => remove(rowIndex)}
                    />
                )}
                headerStyle={{ width: '10%', minWidth: '8rem' }}
                bodyStyle={{ textAlign: 'center' }}
            />
        </DataTable>
    );
};

export default EditableTable;