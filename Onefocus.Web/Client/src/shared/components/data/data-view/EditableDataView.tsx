/* eslint-disable @typescript-eslint/no-explicit-any */
import { FieldArray, FieldArrayPath, FieldArrayWithId, FieldValues, Path, useFieldArray, UseFormReturn } from 'react-hook-form';
import { ReactNode, useCallback, useEffect, useState } from 'react';
import { Card, CardOptions, Panel } from '../../panel';
import DataView from './DataView';
import { PanelHeaderTemplateOptions } from './interfaces';
import { Button } from '../../controls';

type EditableDataViewProps<TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>> = {
    headerName?: string;
    path: TName;
    form: UseFormReturn<TFormInput>;
    inputs: ((index: number, isEditing: boolean) => ReactNode)[];
    newRowValue: FieldArray<TFormInput>;
    isPending?:boolean;
}

interface Action {
    rowId?: string;
    rowIndex: number;
    action?: 'new' | 'edit' | 'remove' | 'accept' | 'cancel';
}

const EditableDataView = <TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>>(props: EditableDataViewProps<TFormInput, TName>) => {
    type EditableRow = FieldArrayWithId<TFormInput, TName, "rowId">;

    const [originalRows, setOriginalRows] = useState<Record<string, any>>({});
    const [rowsState, setRowsState] = useState<Record<string, 'inNewMode' | 'inEditMode' | undefined>>({});
    const [lastAction, setLastAction] = useState<Action>({ rowIndex: 0 });

    const { control, getValues, trigger } = props.form;
    const { fields, update, insert, remove, } = useFieldArray({
        control: control,
        name: props.path,
        keyName: 'rowId'
    });

    const isInFormMode = useCallback((rowId: string) => {
        const rowState = rowsState[rowId];
        if (rowState == 'inNewMode' || rowState == 'inEditMode') return true;

        return false;
    }, [rowsState]);

    //To control useFieldArray's actions
    useEffect(() => {
        switch (lastAction.action) {
            case 'new': {
                const newRowId = fields[lastAction.rowIndex].rowId;
                setRowsState((prev) => ({
                    ...prev,
                    [newRowId]: 'inNewMode'
                }));
                break;
            }
            case 'edit': {
                if (lastAction.rowId) {
                    const rowPathPrefix = `${props.path}.${lastAction.rowIndex}` as Path<TFormInput>;
                    const currentRow = { ...getValues(rowPathPrefix) };

                    setOriginalRows((prev) => {
                        if (!lastAction.rowId) return prev;
                        return {
                            ...prev,
                            [lastAction.rowId]: currentRow
                        };
                    });

                    setRowsState((prev) => {
                        if (!lastAction.rowId) return prev;
                        return {
                            ...prev,
                            [lastAction.rowId]: 'inEditMode'
                        };
                    });
                }
                break;
            }
            case 'remove': {
                remove(lastAction.rowIndex);
                break;
            }
            case 'accept': {
                const rowPathPrefix = `${props.path}.${lastAction.rowIndex}` as Path<TFormInput>;
                const currentRow = getValues(rowPathPrefix);
                const rowFields = Object.keys(currentRow).map(
                    (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                );

                trigger(rowFields).then((value) => {
                    if (value) {
                        setRowsState((prev) => {
                            if (!lastAction.rowId) return prev;
                            const updated = { ...prev };
                            delete updated[lastAction.rowId];
                            return updated;
                        });
                    }
                });
                break;
            }
            case 'cancel': {
                if (lastAction.rowId) {
                    const original = originalRows[lastAction.rowId];
                    if (original) {
                        update(lastAction.rowIndex, original);
                    }

                    setOriginalRows((prev) => {
                        if (!lastAction.rowId) return prev;
                        const updated = { ...prev };
                        delete updated[lastAction.rowId];
                        return updated;
                    });

                    setRowsState((prev) => {
                        if (!lastAction.rowId) return prev;
                        const updated = { ...prev };
                        delete updated[lastAction.rowId];
                        return updated;
                    });
                }
                break;
            }
        }
        if (lastAction.action) {
            setLastAction({ rowIndex: 0 });
        }
    }, [lastAction, fields, getValues, originalRows, props.path, remove, trigger, update]);

    const headerTemplate = useCallback((options: PanelHeaderTemplateOptions) => {
        return (
            <div className={`${options.className} justify-content-space-between`}>
                <div className="flex align-items-center gap-2">
                    <span className="font-bold">{props.headerName}</span>
                </div>
                <div>
                    <Button icon="pi pi-plus" isPending={props.isPending} rounded text onClick={() => {
                        const rowIndex = 0;
                        insert(rowIndex, { ...(props.newRowValue ?? {}) } as EditableRow);
                        setLastAction({
                            action: 'new',
                            rowIndex: rowIndex
                        });
                    }} />
                </div>
            </div>
        );
    }, [props.headerName, props.newRowValue, props.isPending, insert]);


    const cardFooterTemplate = useCallback((options: CardOptions) => {
        const item = fields[options.index] as EditableRow;
        const isFormMode = isInFormMode(item.rowId);
        const isInOnlyEditMode = rowsState[item.rowId] == 'inEditMode';

        const acceptButton = <Button
            icon="pi pi-check"
            rounded
            severity="success"
            onClick={async () => {
                setLastAction({
                    rowId: item.rowId,
                    rowIndex: options.index,
                    action: 'accept'
                });
            }}
        />;

        const cancelButton = <Button
            icon="pi pi-times"
            rounded
            severity="secondary"
            onClick={async () => {
                setLastAction({
                    rowId: item.rowId,
                    rowIndex: options.index,
                    action: 'cancel'
                });
            }}
        />;

        const editButton = <Button
            icon="pi pi-pencil"
            rounded
            severity="info"
            onClick={() => {
                setLastAction({
                    rowId: item.rowId,
                    rowIndex: options.index,
                    action: 'edit'
                });
            }}
        />;

        const removeButton = <Button
            icon="pi pi-trash"
            rounded
            severity="danger"
            onClick={() => {
                setLastAction({
                    rowIndex: options.index,
                    action: 'remove'
                });
            }}
        />

        return (
            <div className='flex gap-3'>
                {!isFormMode && editButton}
                {isFormMode && acceptButton}
                {isInOnlyEditMode && cancelButton}
                {removeButton}
            </div>
        )
    }, [fields, isInFormMode, rowsState]);

    const itemTemplate = useCallback((item: EditableRow) => {
        const index = (fields as (EditableRow[])).findIndex(f => f.rowId == item.rowId);
        const oddRow = index % 2 != 0;

        return (
            <Card key={item.rowId} className={`mb-3 ${oddRow ? 'surface-background-color' : ''}`} footer={cardFooterTemplate} index={index}>
                {props.inputs.map((input) => { return input(index, isInFormMode(item.rowId)); })}
            </Card>
        );
    }, [cardFooterTemplate, fields, props.inputs, isInFormMode]);

    const listTemplate = useCallback((items: typeof fields) => {
        const list = (items ?? []).map((item) => {
            return itemTemplate(item as EditableRow);
        });

        return (
            <>
                {list}
            </>
        );
    }, [itemTemplate]);

    return (
        <Panel
            headerTemplate={headerTemplate}
            toggleable
        >
            <DataView
                value={fields}
                listTemplate={listTemplate}
                emptyMessage="No records found"
                isPending={props.isPending}
            />
        </Panel>
    );
};

export default EditableDataView;
