import { FieldArray, FieldArrayPath, FieldArrayWithId, FieldValues, Path, useFieldArray, UseFormReturn } from 'react-hook-form';
import { ReactNode, useCallback, useState } from 'react';
import { Card, CardOptions, Panel } from '../../panel';
import DataView from './DataView';
import { PanelHeaderTemplateOptions } from './interfaces';
import { Button } from '../../controls';
import { UniqueComponentId } from 'primereact/utils';

type EditableDataViewProps<TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>> = {
    headerName?: string;
    path: TName;
    form: UseFormReturn<TFormInput>;
    inputs: ((index: number, isEditing: boolean) => ReactNode)[];
    newRowValue: FieldArray<TFormInput>;
}

const EditableDataView = <TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>>(props: EditableDataViewProps<TFormInput, TName>) => {
    type EditableRow = FieldArrayWithId<TFormInput, TName, "__internalId"> & { rowId: string };

    const [originalRows, setOriginalRows] = useState<Record<string, FieldArray<TFormInput, TName>>>({});
    const [editingRows, setEditingRows] = useState<Record<string, boolean>>({});

    const { control, getValues, trigger } = props.form;
    const { fields, update, insert, remove } = useFieldArray({
        control: control,
        name: props.path,
        keyName: '__internalId'
    });

    const isEditing = useCallback((rowId: string) => {
        return !!editingRows[rowId];
    }, [editingRows])

    const headerTemplate = useCallback((options: PanelHeaderTemplateOptions) => {
        return (
            <div className={`${options.className} justify-content-space-between`}>
                <div className="flex align-items-center gap-2">
                    <span className="font-bold">{props.headerName}</span>
                </div>
                <div>
                    <Button icon="pi pi-plus" rounded text onClick={() => {
                        const newRow = (props.newRowValue !== null ? { ...props.newRowValue, rowId: UniqueComponentId() } : {}) as EditableRow;
                        insert(0, newRow);
                        setEditingRows((prev) => ({
                            ...prev,
                            [newRow.rowId]: true
                        }));
                    }} />
                </div>
            </div>
        );
    }, [props.headerName, props.newRowValue, insert]);

    const cardFooterTemplate = useCallback((options: CardOptions) => {
        const item = fields[options.index] as EditableRow;
        const isEditMode = isEditing(item.rowId);

        let buttons = <></>;
        if (isEditMode) {
            buttons = (
                <>
                    <Button
                        icon="pi pi-check"
                        rounded
                        severity="success"
                        onClick={async () => {
                            const rowPathPrefix = `${props.path}.${options.index}` as Path<TFormInput>;
                            const currentRow = getValues(rowPathPrefix);
                            const rowFields = Object.keys(currentRow).map(
                                (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                            );

                            const isValid = await trigger(rowFields);
                            if (!isValid) {
                                return;
                            }

                            const updatedItem = getValues(props.path as Path<TFormInput>);
                            update(options.index, updatedItem[options.index]);

                            setEditingRows((prev) => {
                                const updated = { ...prev };
                                delete updated[item.rowId];
                                return updated;
                            });
                        }}
                    />
                    <Button
                        icon="pi pi-times"
                        rounded
                        severity="secondary"
                        onClick={async () => {
                            const rowPathPrefix = `${props.path}.${options.index}` as Path<TFormInput>;
                            const currentRow = getValues(rowPathPrefix);
                            const rowFields = Object.keys(currentRow).map(
                                (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                            );

                            const isValid = await trigger(rowFields);
                            if (!isValid) {
                                return;
                            }

                            const original = originalRows[item.rowId];
                            if (original) {
                                update(options.index, original);
                            }

                            setEditingRows((prev) => {
                                const updated = { ...prev };
                                delete updated[item.rowId];  
                                return updated;
                            });
                        }}
                    />
                </>
            )
        }
        else {
            buttons = (
                <Button
                    icon="pi pi-pencil"
                    rounded
                    severity="info"
                    onClick={() => {
                        const arrayItems = getValues(props.path as Path<TFormInput>);
                        setOriginalRows((prev) => ({
                            ...prev,
                            [item.rowId]: { ...arrayItems[options.index] }
                        }));

                        setEditingRows((prev) => ({
                            ...prev,
                            [item.rowId]: true
                        }));
                    }}
                />

            )
        }

        return (
            <div className='flex gap-3'>
                {buttons}
                <Button
                    icon="pi pi-trash"
                    rounded
                    severity="danger"
                    onClick={() => {
                        remove(options.index);
                    }}
                />
            </div>
        )
    }, [fields, isEditing]);

    const itemTemplate = useCallback((item: EditableRow) => {
        const index = (fields as (EditableRow[])).findIndex(f => f.rowId == item.rowId);
        const oddRow = index % 2 != 0;

        return (
            <Card key={item.rowId} className={`mb-3 ${oddRow ? 'surface-background-color' : ''}`} footer={cardFooterTemplate} index={index}>
                {props.inputs.map((input) => { return input(index, isEditing(item.rowId)); })}
            </Card>
        );
    }, [cardFooterTemplate, fields, props.inputs, isEditing]);

    const listTemplate = useCallback((items: typeof fields) => {
        if (!items || items.length === 0) return null;

        const list = items.map((item) => {
            return itemTemplate(item as EditableRow);
        });

        return (
            <Panel
                headerTemplate={headerTemplate}
                toggleable
            >
                {list}
            </Panel>
        );
    }, [headerTemplate, itemTemplate]);

    return (
        <DataView
            value={fields}
            listTemplate={listTemplate}
        />
    );
};

export default EditableDataView;
