import { Calendar } from 'primereact/calendar';
import { InputProps } from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';
import React, {ReactNode} from 'react';
import {DropdownOption} from './Dropdown';

export type DatePickerProps = InputProps & {
    value?: Date | null;
    onChange?: (e: { value: Date | null | undefined }) => void;
    showTime?: boolean;
    hourFormat?: '12' | '24';
    minDate?: Date;
    maxDate?: Date;
    placeholder?: string;
    dateFormat?: string;
    showSeconds?: boolean;
    onChange?: React.ChangeEvent<HTMLInputElement>;
    onValueChange?: (value: Date | null) => void;
    itemTemplate?: (option: DropdownOption) => ReactNode;
};

export const DatePicker = (props: DatePickerProps) => {
    return (
        <InputWrapper {...props}>
            <Calendar
                id={props.id}
                className={props.className}
                value={props.value}
                onChange={(e) => {
                    if(props.onValueChange) props.onValueChange(e.target.value);
                    if(props.onChange) props.onChange({
                        target: {
                            name: e.target.name,
                            value: e.target.value
                        }
                    });
                }}
                showTime={props.showTime}
                hourFormat={props.hourFormat}
                minDate={props.minDate}
                maxDate={props.maxDate}
                placeholder={props.placeholder}
                dateFormat={props.dateFormat}
                showIcon={true}
                showSeconds={props.showSeconds}
                readOnlyInput={props.readOnly}
                disabled={props.isPending}
                appendTo="self"
                invalid={props.invalid}
            />
        </InputWrapper>
    );
};