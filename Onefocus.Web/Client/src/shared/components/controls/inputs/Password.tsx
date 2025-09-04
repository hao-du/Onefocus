import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import InputWrapper from "./InputWrapper";
import { InputProps, InputWrapperProps } from "../../props";
import { Password as PrimePassword } from 'primereact/password';

type PasswordProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        feedback?: boolean;
    };

const Password = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: PasswordProps<TFieldValues, TName, TTransformedValues>) => {
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
                        <PrimePassword
                            id={controller.field.name}
                            inputId={controller.field.name}
                            onChange={(e) => { controller.field.onChange(e.target.value); }}
                            invalid={controller.fieldState.invalid}
                            value={controller.field.value}
                            readOnly={props.isPending || props.readOnly}
                            autoComplete="current-password"
                            feedback={props.feedback ?? false}
                            inputClassName={props.className}
                        />
                    </InputWrapper>);
            }}
        />
    );
};

export default Password;