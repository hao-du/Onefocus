import {Column as PiColumn, ColumnBodyOptions} from 'primereact/column';

type ColumnProps<TValue> = {
    field? : string;
    header?: string;
    headerStyle?: React.CSSProperties;
    body?: React.ReactNode | ((data: TValue, options: ColumnBodyOptions) => React.ReactNode);
};

export const Column = <TValue,> (props : ColumnProps<TValue>) => {
    return (
        <PiColumn
            field={props.field}
            header={props.header}
            headerStyle={props.headerStyle}
            body={props.body}
        />
    );
}

import {Column as OneFocusColumn, ColumnProps as OneFocusColumnProps} from "../../data/Column";
import {Controller, FieldPath, FieldValues, UseControllerProps} from "react-hook-form";

type ColumnProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusColumnProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

export const Number = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: NumberProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
            render={(controller) => {
                return <OneFocusNumber
                    {...props}
                    {...controller.field}
                    id={controller.field.name}
                    onValueChange={(value) => controller.field.onChange(value)}
                    invalid={controller.fieldState.invalid}
                    errorMessage={controller.fieldState.error?.message}
                />
            }}
        />
    );
};