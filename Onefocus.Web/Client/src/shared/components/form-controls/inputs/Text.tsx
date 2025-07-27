import {
    Text as OneFocusText,
    TextProps as OneFocusTextProps
} from "../../controls";
import { Controller, FieldPath, FieldValues, UseControllerProps } from "react-hook-form";

type TextProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusTextProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const Text = <
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
                    onValueChange={(value) => controller.field.onChange(value)}
                    errorMessage={controller.fieldState.error?.message}
                />
            }}
        />
    );
};

export default Text;