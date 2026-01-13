import { Form } from "antd";
import { Controller, FieldPath, FieldPathValue, FieldValues, UseControllerProps } from "react-hook-form";
import { LabelProps } from "../../../props/BaseProps";
import Select, { SelectProps } from "../../atoms/inputs/Select";

interface FormSelectProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends
    UseControllerProps<TFieldValues, TName, TTransformedValues>,
    Omit<SelectProps, 'name'>,
    LabelProps {
    defaultValue?: FieldPathValue<TFieldValues, TName>;
    required?: boolean;
}

const FormSelect = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormSelectProps<TFieldValues, TName, TTransformedValues>) => {
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
                        <Select
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

export default FormSelect;