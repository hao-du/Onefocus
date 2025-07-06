import {Textarea as OneFocusTextarea, TextareaProps as OneFocusTextareaProps} from "../../controls/inputs/Textarea";
import {Controller, FieldPath, FieldValues, UseControllerProps} from "react-hook-form";

export type TextareaProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusTextareaProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

export const Textarea = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: TextareaProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
            render={(controller) => {
                return <OneFocusTextarea
                    {...props}
                    {...controller.field}
                    id={controller.field.name}
                    invalid={controller.fieldState.invalid}
                    onValueChange={(value) => controller.field.onChange(value)}
                    errorMessage={controller.fieldState.error?.message}
                />
            }}
        />
    );
};