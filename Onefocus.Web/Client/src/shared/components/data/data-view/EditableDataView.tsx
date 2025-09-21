import { FieldArray, FieldArrayPath, FieldArrayWithId, FieldValues, useFieldArray, UseFormReturn } from 'react-hook-form';
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
    const [originalRows, setOriginalRows] = useState<Record<string, FieldArray<TFormInput, TName>>>({});
    const [editingRows, setEditingRows] = useState<Record<string, boolean>>({});

    const { control, getValues, trigger } = props.form;
    const { fields, update, insert, remove } = useFieldArray({
        control: control,
        name: props.path,
        keyName: 'rowId'
    });

    const headerTemplate = useCallback((options: PanelHeaderTemplateOptions) => {
        return (
            <div className={`${options.className} justify-content-space-between`}>
                <div className="flex align-items-center gap-2">
                    <span className="font-bold">{props.headerName}</span>
                </div>
                <div>
                    <Button icon="pi pi-plus" rounded text onClick={() => {
                        const newValue = props.newRowValue !== null
                            ? { ...props.newRowValue, rowId: UniqueComponentId() }
                            : {} as FieldArray<TFormInput, TName> & { rowId: string };
                        insert(0, newValue);

                        setEditingRows((prev) => ({
                            ...prev,
                            [newValue.rowId]: true
                        }));
                    }} />
                </div>
            </div>
        );
    }, [insert, props.headerName, props.newRowValue]);

    const cardFooterTemplate = useCallback((options: CardOptions) => {
        return (
            <div className='flex gap-3'>
                <Button
                    icon="pi pi-pencil"
                    rounded
                    severity="info"
                    onClick={() => {
                    }}
                />
                <Button
                    icon="pi pi-trash"
                    rounded
                    severity="danger"
                    onClick={() => {
                    }}
                />
            </div>
        )
    }, []);

    const itemTemplate = useCallback((item: FieldArrayWithId<TFormInput, TName, "rowId">) => {
        const index = fields.findIndex(f => f.rowId == item.rowId);
        const oddRow = index % 2 != 0;
        const isEditing = !!editingRows[item.rowId];

        return (
            <Card className={`mb-3 ${oddRow ? 'surface-background-color' : ''}`} footer={cardFooterTemplate} index={index}>
                {props.inputs.map((input) => { return input(index, isEditing); })}
            </Card>
        );
    }, [cardFooterTemplate, editingRows, fields, props.inputs]);

    const listTemplate = useCallback((items: typeof fields) => {
        if (!items || items.length === 0) return null;

        const list = items.map((item) => {
            return itemTemplate(item);
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
