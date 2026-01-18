/* eslint-disable @typescript-eslint/no-explicit-any */

import { ReactNode } from "react";
import { Control, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";

interface FormRepeaterProps<
    TDataSource,
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends UseControllerProps<TFieldValues, TName, TTransformedValues> {
    dataSource?: TDataSource[],
    render: (record: TDataSource, control: Control<TFieldValues, any, TTransformedValues> | undefined, index: number) => ReactNode;
}
const FormRepeater = <
    TDataSource,
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormRepeaterProps<TDataSource, TFieldValues, TName, TTransformedValues>) => {
    if (!props.dataSource || !props.control) return null;
    return (
        <>
            {props.dataSource.map((record, index) => {
                return props.render(record, props.control, index);
            })}
        </>
    );
};

export default FormRepeater;