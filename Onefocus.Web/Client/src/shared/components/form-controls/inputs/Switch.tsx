import { 
    Switch as OneFocusSwitch,
    SwitchProps as OneFocusSwitchProps
} from "../../controls";
import {Controller, FieldPath, FieldValues, UseControllerProps} from "react-hook-form";

export type SwitchProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
    > = OneFocusSwitchProps & UseControllerProps<TFieldValues, TName, TTransformedValues>;

const Switch = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
    >(props: SwitchProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller name={props.name} control={props.control} rules={props.rules}
                    render={(controller) => {
                        return <OneFocusSwitch
                            {...props}
                            {...controller.field}
                            id={controller.field.name}
                            checked={controller.field.value}
                            onValueChange={(value) => controller.field.onChange(value)}
                        />
                    }}
        />
    );
};

export default Switch;