import {Text as OneFocusText, TextProps as OneFocusTextProps} from "../../controls/inputs/Text";
import {Controller, FieldPath, FieldValues, UseControllerProps} from "react-hook-form";

export type TextProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusTextProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

export const Text = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(props: TextProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => {
                return <OneFocusText
                    {...props}
                    {...controller.field}
                    id={controller.field.name}
                    invalid={controller.fieldState.invalid}
                    errorMessage={controller.fieldState.error?.message}
                />
            }}
        />
    );
};