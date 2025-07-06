import { InputSwitchChangeEvent } from "primereact/inputswitch";
import { Switch as OneFocusSwitch } from "../../controls/inputs/Switch";
import {Controller, FieldPath, FieldValues, UseControllerProps} from "react-hook-form";
import { InputProps } from "../../../props/InputProps";

export type SwitchProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
    > = InputProps & UseControllerProps<TFieldValues, TName, TTransformedValues> & {
        invalid?: boolean;
        onChange?(event: InputSwitchChangeEvent): void;
}

export const Switch = <
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