import { Controller, FieldPath, FieldValues, UseControllerProps } from 'react-hook-form';
import { Option } from '..';
import InputWrapper from './InputWrapper';
import { InputProps, InputWrapperProps } from '../../props';
import { ReactNode } from 'react';
import { Calendar } from 'primereact/calendar';
import { useSettings } from '../../../hooks';

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
        appendTo?: 'self' | HTMLElement | (() => HTMLElement);
        itemTemplate?: (option: Option) => ReactNode;
    };

const DatePicker = <TFieldValues extends FieldValues = FieldValues, TName extends FieldPath<TFieldValues> = FieldPath<TFieldValues>, TTransformedValues = TFieldValues>
    (props: DateTimeProps<TFieldValues, TName, TTransformedValues>) => {

    const { formatDateTime, settings } = useSettings();
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
                            ? <p className={`${props.size == 'small' ? 'text-sm' : ''} m-2`}>{controller.field.value ? formatDateTime(controller.field.value, props.showTime ?? false) : ''}</p>
                            : <Calendar
                                id={controller.field.name}
                                onChange={(e) => { controller.field.onChange(e.value); }}
                                locale={settings?.locale}
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
                                appendTo={props.appendTo ?? document.body}
                                readOnlyInput={props.readOnly}
                                disabled={props.isPending}
                                className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
                            />
                        }
                    </InputWrapper>);
            }}
        />
    );
};

export default DatePicker;