import { Calendar } from 'primereact/calendar';
import { InputProps } from '../../props';
import InputWrapper from './InputWrapper';
import { ReactNode } from 'react';
import { Option } from '../interfaces';

export type DatePickerProps = InputProps & {
    value?: Date | null;
    showTime?: boolean;
    hourFormat?: '12' | '24';
    minDate?: Date;
    maxDate?: Date;
    placeholder?: string;
    dateFormat?: string;
    showSeconds?: boolean;
    onValueChange?: (value: Date | null | undefined) => void;
    itemTemplate?: (option: Option) => ReactNode;
};

const DatePicker = (props: DatePickerProps) => {
    return (
        <InputWrapper {...props}>
            <Calendar
                id={props.id}
                className={props.className}
                value={props.value}
                onChange={(e) => {
                    if (props.onValueChange) props.onValueChange(e.target.value);
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

export default DatePicker;