import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import { Option } from '..';
import InputWrapper from './InputWrapper';
import { InputProps, InputWrapperProps } from '../../props';
import { ReactNode } from 'react';
import { Calendar } from 'primereact/calendar';

export type DateTimeProps<TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    = UseControllerProps<TFieldValues, TName, TTransformedValues>
    & InputProps
    & InputWrapperProps
    & {
        value?: Date | null;
        showTime?: boolean;
        showSeconds?: boolean;
        hourFormat?: '12' | '24';
        dateFormat?: string;
        minDate?: Date;
        maxDate?: Date;
        placeholder?: string;
        itemTemplate?: (option: Option) => ReactNode;
    };

const DatePicker = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: DateTimeProps<TFieldValues, TName, TTransformedValues>) => {
    return (
        <Controller
            name={props.name}
            control={props.control}
            rules={props.rules}
            render={(controller) => {
                return (
                    <InputWrapper
                        htmlFor={controller.field.name}
                        errorMessage={controller.fieldState.error?.message}
                        description={props.description}
                    >
                        <Calendar
                            id={controller.field.name}
                            onChange={(e) => { controller.field.onChange(e.target.value); }}
                            invalid={controller.fieldState.invalid}
                            value={controller.field.value ? new Date(controller.field.value) : undefined}
                            showTime={props.showTime}
                            hourFormat={props.hourFormat}
                            minDate={props.minDate}
                            maxDate={props.maxDate}
                            placeholder={props.placeholder}
                            dateFormat={props.dateFormat}
                            showIcon={true}
                            showSeconds={props.showSeconds}
                            appendTo="self"
                            readOnlyInput={props.readOnly}
                            disabled={props.isPending}
                            className={props.className}
                        />
                    </InputWrapper>);
            }}
        />
    );
};

export default DatePicker;