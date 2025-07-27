import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import {
    DatePicker as OnefocusDatePicker,
    DatePickerProps as OneFocusDatePickerProps
} from '../../controls';

export type DateTimeProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusDatePickerProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const DatePicker = <
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
                    onValueChange={(value) => controller.field.onChange(value)}
                    invalid={controller.fieldState.invalid}
                    errorMessage={controller.fieldState.error?.message}
                />
            )}
        />
    );
};

export default DatePicker;