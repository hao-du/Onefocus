import { Calendar } from 'primereact/calendar';
import { InputProps } from '../../../props/InputProps';
import {InputWrapper} from './InputWrapper';

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
};

export const DatePicker = (props: DatePickerProps) => {
    return (
        <InputWrapper {...props}>
            <Calendar
                id={props.id}
                className={props.className}
                value={props.value}
                onChange={(e) => {
                    if(props.onChange) props.onChange(e);
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
            />
        </InputWrapper>
    );
};