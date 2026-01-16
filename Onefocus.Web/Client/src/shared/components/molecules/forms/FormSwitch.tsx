import { Controller, FieldPath, FieldPathValue, FieldValues, UseControllerProps } from "react-hook-form";
import Switch, { SwitchProps } from "../../atoms/inputs/Switch";
import { Form } from "antd";
import { LabelProps } from "../../../props/BaseProps";


interface FormSwitchProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> extends
    UseControllerProps<TFieldValues, TName, TTransformedValues>,
    Omit<SwitchProps, 'name'>,
    LabelProps {
    defaultValue?: FieldPathValue<TFieldValues, TName>;
    required?: boolean;
    extra?: string;
}

const FormSwitch = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: FormSwitchProps<TFieldValues, TName, TTransformedValues>) => {
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
                        extra={props.extra}
                    >
                        <Switch
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

export default FormSwitch;