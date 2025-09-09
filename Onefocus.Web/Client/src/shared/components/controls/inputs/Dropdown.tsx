import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import { Dropdown as PrimeDropdown } from 'primereact/dropdown';
import InputWrapper from './InputWrapper';
import { InputProps, InputWrapperProps } from '../../props';
import { ReactNode } from 'react';
import { Option } from '..';

export type DropdownProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        options: Option[];
        autoComplete?: 'on' | 'off' | string;
        value?: string | number | boolean | null;
        itemTemplate?: (option: Option) => ReactNode;
    };

const Dropdown = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: DropdownProps<TFieldValues, TName, TTransformedValues>) => {
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
                        <PrimeDropdown
                            id={controller.field.name}
                            onChange={(e) => { controller.field.onChange(e.value); }}
                            invalid={controller.fieldState.invalid}
                            value={controller.field.value}
                            options={props.options}
                            itemTemplate={props.itemTemplate}
                            placeholder={props.placeholder}
                            readOnly={props.isPending || props.readOnly}
                            disabled={props.isPending || props.disabled}
                            autoComplete={props.autoComplete}
                            filter
                            checkmark={true}
                            highlightOnSelect={true}
                            className={props.className}
                        />
                    </InputWrapper>);
            }}
        />
    );
};

export default Dropdown;