import { DatePicker as AntDatePicker } from 'antd';
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";
import { joinClassNames } from '../../../utils';

export interface DatePickerProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    picker?: 'time' | 'date' | 'week' | 'month' | 'quarter' | 'year';
    showTime?: boolean;
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string | null) => void;
    value?: string;
}

const DatePicker = (props: DatePickerProps) => {
    return (
        <AntDatePicker
            key={props.key}
            id={props.id}
            name={props.name}
            value={props.value}
            className={joinClassNames('w-full', props.className)}
            picker={props.picker}
            showTime={props.showTime}
            size={props.size}
            status={props.status}
            disabled={props.disabled || props.isPending}
            autoComplete={props.autoComplete}
            placeholder={props.placeHolder}
            onChange={(value) => {
                if (props.onChange) props.onChange(value);
            }}
        />
    );
};

export default DatePicker;