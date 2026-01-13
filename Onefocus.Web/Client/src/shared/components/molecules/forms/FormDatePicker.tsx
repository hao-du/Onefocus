import { Form } from "antd";
import { Controller, FieldPath, FieldPathValue, FieldValues, UseControllerProps } from "react-hook-form";
import { LabelProps } from "../../../props/BaseProps";
import DatePicker, { DatePickerProps } from "../../atoms/inputs/DatePicker";

interface FormDatePickerProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends
    UseControllerProps<TFieldValues, TName, TTransformedValues>,
    Omit<DatePickerProps, 'name'>,
    LabelProps {
    defaultValue?: FieldPathValue<TFieldValues, TName>;
    required?: boolean;
}

const FormDatePicker = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormDatePickerProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => {
                return (
                    <Form.Item
                        colon={false}
                        label={props.label}
                        htmlFor={props.id ?? props.name}
                        validateStatus={controller.fieldState.error ? 'error' : ''}
                        help={controller.fieldState.error?.message}
                        required={Boolean(props.rules?.required)}
                    >
                        <DatePicker
                            {...props}
                            value={controller.field.value}
                            id={props.id ?? props.name}
                            onChange={(value) => {
                                controller.field.onChange(value);
                                if (props.onChange) props.onChange(value);
                            }}
                        />
                    </Form.Item>
                );
            }}
        />
    );
};

export default FormDatePicker;