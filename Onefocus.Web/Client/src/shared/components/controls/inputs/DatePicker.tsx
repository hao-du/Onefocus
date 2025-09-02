import { Calendar } from 'primereact/calendar';
import { ReactNode } from 'react';
import { InputProps } from '../../props';
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
        <Calendar
            id={props.id}
            className={props.className}
            value={props.value ? new Date(props.value) : undefined}
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
    );
};

export default DatePicker;