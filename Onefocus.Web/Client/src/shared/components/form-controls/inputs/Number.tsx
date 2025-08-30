import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import {
    Number as OneFocusNumber,
    NumberProps as OneFocusNumberProps
} from "../../controls";
import InputWrapper from "./InputWrapper";

type NumberProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusNumberProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const Number = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: NumberProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
            render={(controller) => {
                return (
                    <InputWrapper {...props} errorMessage={controller.fieldState.error?.message} invalid={controller.fieldState.invalid}>
                        <OneFocusNumber
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

export default Number;