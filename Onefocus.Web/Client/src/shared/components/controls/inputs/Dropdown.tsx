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
        filter?: boolean;
    };

const Dropdown = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: DropdownProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.textOnly ? undefined : props.rules}
            render={(controller) => {
                return (
                    <InputWrapper
                        {...props}
                        htmlFor={controller.field.name}
                        errorMessage={controller.fieldState.error?.message}
                    >
                        {props.textOnly
                            ? <p className={`${props.size == 'small' ? 'text-sm' : ''} m-2`}>{props.options.find(o => o.value == controller.field.value)?.label ?? ''}</p>
                            : <PrimeDropdown
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
                                filter={props.filter ?? true}
                                checkmark={true}
                                highlightOnSelect={true}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
                            />
                        }
                    </InputWrapper>);
            }}
        />
    );
};

export default Dropdown;