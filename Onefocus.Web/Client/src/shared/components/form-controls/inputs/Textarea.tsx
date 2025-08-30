import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";
import {
    Textarea as OneFocusTextarea,
    TextareaProps as OneFocusTextareaProps
} from "../../controls";
import InputWrapper from "./InputWrapper";

type TextareaProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusTextareaProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const Textarea = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: TextareaProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
            render={(controller) => {
                return (
                    <InputWrapper {...props} errorMessage={controller.fieldState.error?.message} invalid={controller.fieldState.invalid}>
                        <OneFocusTextarea
                            {...props}
                            {...controller.field}
                            id={controller.field.name}
                            invalid={controller.fieldState.invalid}
                            onValueChange={(value) => controller.field.onChange(value)}
                        />
                    </InputWrapper>);
            }}
        />
    );
};

export default Textarea;