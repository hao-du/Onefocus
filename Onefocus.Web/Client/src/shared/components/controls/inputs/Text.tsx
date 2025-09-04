import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputText } from "primereact/inputtext";
import { InputProps, InputWrapperProps } from "../../props";

type TextProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues> 
    & InputProps
    & InputWrapperProps
    & {
        autoComplete?: 'username' | string;
    };

const Text = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: TextProps<TFieldValues, TName, TTransformedValues>) => {
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
                        <InputText
                            id={controller.field.name}
                            onChange={(e) => { controller.field.onChange(e.target.value); }}
                            invalid={controller.fieldState.invalid}
                            value={controller.field.value}
                            readOnly={props.isPending || props.readOnly}
                            autoComplete={props.autoComplete}
                            className={props.className}
                        />
                    </InputWrapper>
                );
            }}
        />
    );
};

export default Text;