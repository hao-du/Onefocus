import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputProps, InputWrapperProps } from "../../props";
import { InputSwitch } from "primereact/inputswitch";

export type SwitchProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        checked?: boolean;
    };

const Switch = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: SwitchProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => {
                return (
                    <InputWrapper
                        label={props.label}
                        htmlFor={controller.field.name}
                        errorMessage={controller.fieldState.error?.message}
                        description={props.description}
                    >
                        <InputSwitch
                            id={controller.field.name}
                            onChange={(e) => { controller.field.onChange(e.target.value); }}
                            invalid={controller.fieldState.invalid}
                            checked={controller.field.value ?? false}
                            disabled={props.isPending || props.readOnly}
                            className={props.className}
                        />
                    </InputWrapper>);
            }}
        />
    );
};

export default Switch;