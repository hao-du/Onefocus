import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import {
    Password as OneFocusPassword,
    PasswordProps as OneFocusPasswordProps
} from "../../controls";
import InputWrapper from "./InputWrapper";

type PasswordProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusPasswordProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const Password = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: PasswordProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
            render={(controller) => {
                return (<InputWrapper {...props} errorMessage={controller.fieldState.error?.message} invalid={controller.fieldState.invalid}>
                    <OneFocusPassword
                        {...props}
                        {...controller.field}
                        id={controller.field.name}
                        onValueChange={(value) => controller.field.onChange(value)}
                        invalid={controller.fieldState.invalid}
                    />
                </InputWrapper>);
            }}
        />
    );
};

export default Password;