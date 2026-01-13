import { InputNumber } from "antd";
import { ClassNameProps, FocusProps, IdentityProps, InteractionProps, NameProps } from "../../../props/BaseProps";
import { SizeType } from "antd/es/config-provider/SizeContext";
import { StateType } from "../../../types";
import { joinClassNames } from "../../../utils";

export interface NumberProps extends ClassNameProps, IdentityProps, InteractionProps, NameProps, FocusProps {
    autoComplete?: string;
    placeHolder?: string;
    size?: SizeType;
    defaultValue?: string | number | readonly string[];
    status?: Exclude<StateType, 'info'> | 'validating';
    onChange?: (value: string | null) => void;
    value?: string;
}

const Number = (props: NumberProps) => {
    return (
        <InputNumber
            key={props.key}
            id={props.id}
            name={props.name}
            value={props.value}
            style={{ width: '100%' }}
            formatter={(value) => {
                const [start, end] = `${value}`.split('.') || [];
                const v = `${start}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                return `${end ? `${v}.${end}` : `${v}`}`;
            }}
            precision={2}
            parser={(value) => value?.replace(/\$\s?|(,*)/g, '') ?? ''}
            controls={false}
            className={joinClassNames('text-right', props.className)}
            size={props.size}
            status={props.status}
            disabled={props.disabled || props.isPending}
            autoComplete={props.autoComplete}
            placeholder={props.placeHolder}
            onFocus={(e) => { if (props.focus) e.target.select(); }}
            onChange={(value) => {
                if (props.onChange) props.onChange(value);
            }}
        />
    );
};

export default Number;