import { Form } from "antd";
import { Controller, FieldPath, FieldPathValue, FieldValues, UseControllerProps } from "react-hook-form";
import { LabelProps, ReadOnlyProps } from "../../../props/BaseProps";
import TextArea, { TextAreaProps } from "../../atoms/inputs/TextArea";

interface FormTextAreaProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends
    UseControllerProps<TFieldValues, TName, TTransformedValues>,
    Omit<TextAreaProps, 'name' | 'readOnly'>,
    LabelProps,
    ReadOnlyProps {
    defaultValue?: FieldPathValue<TFieldValues, TName>;
    required?: boolean;
}

const FormTextArea = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormTextAreaProps<TFieldValues, TName, TTransformedValues>) => {
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
                        {props.readOnly && (
                            <TextArea
                                {...props}
                                value={controller.field.value}
                                id={props.id ?? props.name}
                                readOnly
                            />
                        )}
                        {!props.readOnly && (
                            <TextArea
                                {...props}
                                value={controller.field.value}
                                id={props.id ?? props.name}
                                onChange={(value) => {
                                    controller.field.onChange(value);
                                    if (props.onChange) props.onChange(value);
                                }}
                            />
                        )}
                    </Form.Item>
                );
            }}
        />
    );
};

export default FormTextArea;