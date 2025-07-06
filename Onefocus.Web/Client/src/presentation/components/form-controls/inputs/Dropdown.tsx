import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import {
    Dropdown as OneFocusDropdown,
    DropdownProps as OneFocusDropdownProps,
    DropdownOption
} from '../../controls/inputs/Dropdown';

export type DropdownProps<
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
> = OneFocusDropdownProps & UseControllerProps<TFieldValues, TName, TTransformedValues> & {
    options: DropdownOption[];
};

export const Dropdown = <
    TFieldValues extends FieldValues = FieldValues,
    TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>,
    TTransformedValues = TFieldValues
>(
    props: DropdownProps<TFieldValues, TName, TTransformedValues>
) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => (
                <OneFocusDropdown
                    {...props}
                    {...controller.field}
                    id={controller.field.name}
                    value={controller.field.value}
                    onChange={(value) => controller.field.onChange(value)}
                    invalid={controller.fieldState.invalid}
                    errorMessage={controller.fieldState.error?.message}
                />
            )}
        />
    );
};