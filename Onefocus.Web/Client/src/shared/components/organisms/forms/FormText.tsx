import { Form } from "antd";
import { Controller, FieldPath, FieldPathValue, FieldValues, UseControllerProps } from "react-hook-form";
import Text, { TextProps } from "../../atoms/inputs/Text";
import { LabelProps } from "../../../props/BaseProps";

interface FormTextProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends
    UseControllerProps<TFieldValues, TName, TTransformedValues>,
    TextProps,
    LabelProps {
    defaultValue?: FieldPathValue<TFieldValues, TName>;
    required?: boolean;
}

const FormText = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormTextProps<TFieldValues, TName, TTransformedValues>) => {
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
                        validateStatus={controller.fieldState.error ? 'error' : ''}
                        help={controller.fieldState.error?.message}
                        required={props.required}
                    >
                        <Text
                            {...props}
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

export default FormText;