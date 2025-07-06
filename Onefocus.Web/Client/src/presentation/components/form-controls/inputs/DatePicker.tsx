import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import {
    DatePicker as OnefocusDatePicker,
    DatePickerProps as OneFocusDatePickerProps
} from '../../controls/inputs/DatePicker';

export type DateTimeProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusDatePickerProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

export const DatePicker = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(
    props: DateTimeProps<TFieldValues, TName, TTransformedValues>
) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => (
                <OnefocusDatePicker
                    {...props}
                    id={controller.field.name}
                    value={controller.field.value}
                    onChange={(value) => controller.field.onChange(value)}
                    invalid={controller.fieldState.invalid}
                    errorMessage={controller.fieldState.error?.message}
                />
            )}
        />
    );
};