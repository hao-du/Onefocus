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
            rules={props.textOnly ? undefined : props.rules}
            render={(controller) => {
                return (
                    <InputWrapper
                        {...props}
                        htmlFor={controller.field.name}
                        errorMessage={controller.fieldState.error?.message}
                    >
                        {props.textOnly
                            ? <i className={`pi ${controller.field.value ? 'pi-check-circle' : 'pi-ban'}`}></i>
                            : <InputSwitch
                                id={controller.field.name}
                                onChange={(e) => { controller.field.onChange(e.target.value); }}
                                invalid={controller.fieldState.invalid}
                                checked={controller.field.value ?? false}
                                disabled={props.isPending || props.readOnly}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputswitch-sm' : ''}`}
                            />
                        }
                    </InputWrapper>);
            }}
        />
    );
};

export default Switch;