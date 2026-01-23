/* eslint-disable @typescript-eslint/no-explicit-any */


import { ReactNode, useCallback, useState } from "react";
import { Control, FieldArray, FieldArrayPath, FieldArrayWithId, FieldValues, Path, PathValue, useFieldArray, UseFormReturn } from "react-hook-form";
import Card from "../panels/Card";
import Button from "../../atoms/buttons/Button";
import { getGuid, joinClassNames } from "../../../utils";
import Space from "../../atoms/panels/Space";
import Icon from "../../atoms/misc/Icon";

interface FormRepeaterProps<TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>> {
    title?: string;
    path: TName;
    form: UseFormReturn<TFormInput>;
    render: (record: FieldArray<TFormInput, TName>, control: Control<TFormInput, any, TFormInput> | undefined, index: number, isReadMode: boolean, isFocused: boolean) => ReactNode;
    defaultRowValue: FieldArray<TFormInput>;
    isPending?: boolean;
}

interface RowAction {
    rowIndex: number;
    action?: 'new' | 'edit' | 'remove' | 'accept' | 'cancel';
}

const INTERAL_ROW_ID = 'interalRowId';

const FormRepeater = <TFormInput extends FieldValues, TName extends FieldArrayPath<TFormInput>>(props: FormRepeaterProps<TFormInput, TName>) => {
    type RowMode = 'edit-new' | 'edit-existing';
    type RowMeta = FieldArrayWithId<TFormInput, TName, typeof INTERAL_ROW_ID> & {
        id?: string;
        rowId?: string;
        mode?: RowMode;
    };

    const [backupRows, setBackupRows] = useState<Record<string, any>>({});
    const [focusedRowIndex, setFocusedRowIndex] = useState<number>(-1);

    const { control, setValue, getValues, trigger } = props.form;
    const { fields, update, insert, remove, } = useFieldArray({
        control: control,
        name: props.path,
        keyName: INTERAL_ROW_ID
    });

    const switchAction = useCallback((rowAction: RowAction) => {
        switch (rowAction.action) {
            case 'new': {
                const rowIndex = 0;
                const rowId = getGuid();
                const defaultValue = props.defaultRowValue ?? {};
                insert(rowIndex, { ...defaultValue, rowId, mode: 'edit-new' } as RowMeta);
                setFocusedRowIndex(rowIndex);
                break;
            }
            case 'edit': {
                const rowMeta = fields[rowAction.rowIndex] as RowMeta;
                const rowId = rowMeta.rowId ?? rowMeta.id;
                if (!rowId) return;

                const rowPathPrefix = `${props.path}.${rowAction.rowIndex}` as Path<TFormInput>;
                const currentRow = getValues(rowPathPrefix);

                setBackupRows((prev) => {
                    return {
                        ...prev,
                        [rowId]: currentRow
                    };
                });

                update(rowAction.rowIndex, {
                    ...currentRow,
                    rowId,
                    mode: 'edit-existing'
                } as RowMeta);

                setFocusedRowIndex(rowAction.rowIndex);
                break;
            }
            case 'remove': {
                remove(rowAction.rowIndex);
                break;
            }
            case 'accept': {
                const rowMeta = fields[rowAction.rowIndex] as RowMeta;

                const rowPathPrefix = `${props.path}.${rowAction.rowIndex}` as Path<TFormInput>;
                const currentRow = getValues(rowPathPrefix);

                const validationFields = Object.keys(currentRow).map(
                    (key) => `${rowPathPrefix}.${key}` as Path<TFormInput>
                );
                trigger(validationFields).then((value) => {
                    if (value) {
                        update(rowAction.rowIndex, {
                            ...currentRow,
                            rowId: rowMeta.rowId,
                            mode: undefined
                        } as RowMeta);
                    }
                });

                break;
            }
            case 'cancel': {
                const rowMeta = fields[rowAction.rowIndex] as RowMeta;

                if (rowMeta.rowId) {
                    const rowMeta = fields[rowAction.rowIndex] as RowMeta;
                    const rowId = rowMeta.rowId ?? rowMeta.id;
                    if (!rowId) return;

                    const originalValues = backupRows[rowId];
                    if (!originalValues) return;

                    Object.entries(originalValues).forEach(([key, value]) => {
                        setValue(
                            `${props.path}.${rowAction.rowIndex}.${key}` as Path<TFormInput>,
                            value as PathValue<TFormInput, Path<TFormInput>>,
                            { shouldDirty: false }
                        );
                    });

                    update(rowAction.rowIndex, {
                        ...originalValues,
                        rowId: rowMeta.rowId,
                        mode: undefined
                    } as RowMeta);

                    setBackupRows((prev) => {
                        if (!rowMeta.rowId) return prev;
                        const updated = { ...prev };
                        delete updated[rowMeta.rowId];
                        return updated;
                    });
                }
                break;
            }
        }
    }, [backupRows, fields, getValues, insert, props.defaultRowValue, props.path, remove, setValue, trigger, update]);

    const isReadMode = useCallback((rowMeta: RowMeta) => {
        return rowMeta.mode == undefined;
    }, []);

    const getActions = useCallback((rowIndex: number) => {
        const rowMeta = fields[rowIndex] as RowMeta;

        const isEditMode = !isReadMode(rowMeta);
        const showCancelButton = rowMeta.mode == 'edit-existing';

        const acceptButton = <Button
            icon={<Icon name="accept" />}
            variant="link"
            color="green"
            onClick={() => switchAction({ rowIndex, action: 'accept' })}
        />;

        const cancelButton = <Button
            icon={<Icon name="cancel" />}
            variant="link"
            color="default"
            onClick={() => switchAction({ rowIndex, action: 'cancel' })}
        />;

        const editButton = <Button
            icon={<Icon name="edit" />}
            variant="link"
            color="primary"
            onClick={() => switchAction({ rowIndex, action: 'edit' })}
        />;

        const removeButton = <Button
            icon={<Icon name="remove" />}
            variant="link"
            color="danger"
            onClick={() => switchAction({ rowIndex, action: 'remove' })}
        />

        return (
            <Space size="small">
                {!isEditMode && editButton}
                {isEditMode && acceptButton}
                {showCancelButton && cancelButton}
                {removeButton}
            </Space>
        )
    }, [fields, isReadMode, switchAction]);

    const getRowMeta = useCallback((field: FieldArray<TFormInput, TName>) => {
        return field as unknown as RowMeta;
    }, []);

    return (
        <Card
            title={props.title}
            titleExtra={
                <Button
                    icon={<Icon name="add" />}
                    variant="link"
                    color="default"
                    onClick={() => {
                        switchAction({ rowIndex: -1, action: 'new' });
                    }}
                />
            }
            body={
                fields.map((item) => {
                    const rowMeta = getRowMeta(item);
                    const index = fields.findIndex(f => (rowMeta.rowId && getRowMeta(f).rowId == rowMeta.rowId) || getRowMeta(f).id == rowMeta.id);
                    return (
                        <Card
                            key={rowMeta.rowId}
                            className={joinClassNames(index == fields.length - 1 ? '' : 'mb-3!', index % 2 == 0 ? 'border-(--ant-color-border)!' : '')}
                            body={props.render(item, control, index, isReadMode(rowMeta), index == focusedRowIndex)}
                            rightActions={getActions(index)}
                        />
                    );
                })
            }
        />
    );
};

export default FormRepeater;